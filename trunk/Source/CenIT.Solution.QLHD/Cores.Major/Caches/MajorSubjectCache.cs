using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Major.Caches
{
    public class MajorSubjectCache : CacheLayer
    {
        private MajorSubjectBiz _subjectApi;
        private MajorSubjectBiz Api => _subjectApi ?? (_subjectApi = new MajorSubjectBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "SubjectsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorSubjectModel> GetAll()
        {
            var rawKey = "AllSubjects-";
            if (GetCacheItem(rawKey) is List<MajorSubjectModel> subjects) return subjects;
            subjects = Api.GetAll();
            AddCacheItem(rawKey, subjects);
            return subjects;
        }

        /// <summary>
        /// Danh sách đối tượng theo tiêu chí tra cứu, đã áp dụng phân quyền dữ liệu.
        /// LƯU Ý: khoá cache BẮT BUỘC phải chứa userName vì kết quả khác nhau theo
        /// từng người dùng; nếu thiếu sẽ gây rò rỉ dữ liệu giữa các tài khoản.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        /// <summary>
        /// So lieu tong hop cho khoi thong ke dau man hinh.
        ///
        /// CO Y KHONG CACHE: so lieu phu thuoc ca bang Doi tuong lan bang Vi pham,
        /// ma hai tang cache dung hai khoa khac nhau (SubjectsCache /
        /// SubjectViolationsCache). Cache o day thi khi ghi nhan vi pham moi, con so
        /// tren the se dung yen trong khi danh sach ben duoi da doi.
        /// Truy van chi ~100ms va chay mot lan moi khi mo man hinh nen doc thang la du.
        /// </summary>
        public MajorSubjectDashboardModel GetDashboard(string userName)
        {
            return Api.GetDashboard(userName) ?? new MajorSubjectDashboardModel();
        }

        public List<MajorSubjectModel> Get(out int total, string identityCardNumber, string fullName,
            string behaviorIds, string gender, string userName, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(identityCardNumber);
            var objectKey3 = EHashMD5.FromObject(fullName);
            var objectKey4 = EHashMD5.FromObject(behaviorIds);
            var objectKey5 = EHashMD5.FromObject(gender);
            var objectKey6 = EHashMD5.FromObject(userName);
            var rawKey = string.Concat("ListSubjects-", objectKey, objectKey2, objectKey3, objectKey4, objectKey5, objectKey6);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<MajorSubjectModel> subjects) return subjects;
            subjects = Api.Get(out total, identityCardNumber, fullName, behaviorIds, gender, userName, search);
            AddCacheItem(rawKey, subjects);
            AddCacheItem(rawKeyTotal, total);
            return subjects;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public MajorSubjectModel GetById(Guid? subjectId)
        {
            var rawKey = string.Concat("SubjectDetail-", subjectId);
            if (GetCacheItem(rawKey) is MajorSubjectModel subject) return subject;
            subject = Api.GetById(subjectId);
            AddCacheItem(rawKey, subject);
            return subject;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public MajorSubjectModel GetByIdentityCardNumber(string identityCardNumber)
        {
            if (string.IsNullOrWhiteSpace(identityCardNumber)) return null;

            var cardNumber = identityCardNumber.Trim();
            var rawKey = string.Concat("SubjectByCard-", EHashMD5.FromObject(cardNumber));
            if (GetCacheItem(rawKey) is MajorSubjectModel subject) return subject;
            subject = Api.GetByIdentityCardNumber(cardNumber);
            AddCacheItem(rawKey, subject);
            return subject;
        }

        /// <summary>
        /// Đọc thẳng từ CSDL, KHÔNG qua cache.
        /// Dùng khi cần so sánh phát hiện thay đổi - nếu đọc từ cache có thể so
        /// với dữ liệu cũ và kết luận sai.
        /// </summary>
        public MajorSubjectModel GetByIdFresh(Guid? subjectId)
        {
            return Api.GetById(subjectId);
        }

        public string Save(MajorSubjectModel model, string username)
        {
            var result = Api.Save(model, username);
            InvalidateCache();
            return result;
        }

        public bool Delete(Guid subjectId, string username)
        {
            var result = Api.Delete(subjectId, username);
            InvalidateCache();
            return result;
        }
    }
}

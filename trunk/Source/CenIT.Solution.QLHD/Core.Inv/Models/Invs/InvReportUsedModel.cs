using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Inv.Models.Invs
{
    [XmlRoot(ElementName = "TTinDVu")]
    public class TTinDVu
    {
        [XmlElement(ElementName = "maDVu")] public string MaDVu { get; set; }

        [XmlElement(ElementName = "tenDVu")] public string TenDVu { get; set; }

        [XmlElement(ElementName = "pbanDVu")] public string PbanDVu { get; set; }

        [XmlElement(ElementName = "ttinNhaCCapDVu")]
        public string TtinNhaCCapDVu { get; set; }
    }

    [XmlRoot(ElementName = "KyKKhaiThue")]
    public class KyKKhaiThue
    {
        [XmlElement(ElementName = "kieuKy")] public string KieuKy { get; set; }

        [XmlElement(ElementName = "kyKKhai")] public string KyKKhai { get; set; }

        [XmlElement(ElementName = "kyKKhaiTuNgay")]
        public string KyKKhaiTuNgay { get; set; }

        [XmlElement(ElementName = "kyKKhaiDenNgay")]
        public string KyKKhaiDenNgay { get; set; }

        [XmlElement(ElementName = "kyKKhaiTuThang")]
        public string KyKKhaiTuThang { get; set; }

        [XmlElement(ElementName = "kyKKhaiDenThang")]
        public string KyKKhaiDenThang { get; set; }
    }

    [XmlRoot(ElementName = "GiaHan")]
    public class GiaHan
    {
        [XmlElement(ElementName = "maLyDoGiaHan")]
        public string MaLyDoGiaHan { get; set; }

        [XmlElement(ElementName = "lyDoGiaHan")]
        public string LyDoGiaHan { get; set; }
    }

    [XmlRoot(ElementName = "TKhaiThue")]
    public class TKhaiThue
    {
        [XmlElement(ElementName = "maTKhai")] public string MaTKhai { get; set; }

        [XmlElement(ElementName = "tenTKhai")] public string TenTKhai { get; set; }

        [XmlElement(ElementName = "moTaBMau")] public string MoTaBMau { get; set; }

        [XmlElement(ElementName = "pbanTKhaiXML")]
        public string PbanTKhaiXML { get; set; }

        [XmlElement(ElementName = "loaiTKhai")]
        public string LoaiTKhai { get; set; }

        [XmlElement(ElementName = "soLan")] public string SoLan { get; set; }

        [XmlElement(ElementName = "KyKKhaiThue")]
        public KyKKhaiThue KyKKhaiThue { get; set; } = new KyKKhaiThue();

        [XmlElement(ElementName = "maCQTNoiNop")]
        public string MaCQTNoiNop { get; set; }

        [XmlElement(ElementName = "tenCQTNoiNop")]
        public string TenCQTNoiNop { get; set; }

        [XmlElement(ElementName = "ngayLapTKhai")]
        public string NgayLapTKhai { get; set; }

        [XmlElement(ElementName = "GiaHan")] public GiaHan GiaHan { get; set; } = new GiaHan();

        [XmlElement(ElementName = "nguoiKy")] public string NguoiKy { get; set; }

        [XmlElement(ElementName = "ngayKy")] public string NgayKy { get; set; }

        [XmlElement(ElementName = "nganhNgheKD")]
        public string NganhNgheKD { get; set; }
    }

    [XmlRoot(ElementName = "NNT")]
    public class NNT
    {
        [XmlElement(ElementName = "mst")] public string Mst { get; set; }

        [XmlElement(ElementName = "tenNNT")] public string TenNNT { get; set; }

        [XmlElement(ElementName = "dchiNNT")] public string DchiNNT { get; set; }

        [XmlElement(ElementName = "phuongXa")] public string PhuongXa { get; set; }

        [XmlElement(ElementName = "maHuyenNNT")]
        public string MaHuyenNNT { get; set; }

        [XmlElement(ElementName = "tenHuyenNNT")]
        public string TenHuyenNNT { get; set; }

        [XmlElement(ElementName = "maTinhNNT")]
        public string MaTinhNNT { get; set; }

        [XmlElement(ElementName = "tenTinhNNT")]
        public string TenTinhNNT { get; set; }

        [XmlElement(ElementName = "dthoaiNNT")]
        public string DthoaiNNT { get; set; }

        [XmlElement(ElementName = "faxNNT")] public string FaxNNT { get; set; }

        [XmlElement(ElementName = "emailNNT")] public string EmailNNT { get; set; }
    }

    [XmlRoot(ElementName = "TTinTKhaiThue")]
    public class TTinTKhaiThue
    {
        [XmlElement(ElementName = "TKhaiThue")]
        public TKhaiThue TKhaiThue { get; set; } = new TKhaiThue();

        [XmlElement(ElementName = "NNT")] public NNT NNT { get; set; } = new NNT();
    }

    [XmlRoot(ElementName = "TTinChung")]
    public class TTinChung
    {
        [XmlElement(ElementName = "TTinDVu")] public TTinDVu TTinDVu { get; set; } = new TTinDVu();

        [XmlElement(ElementName = "TTinTKhaiThue")]
        public TTinTKhaiThue TTinTKhaiThue { get; set; } = new TTinTKhaiThue();
    }

    [XmlRoot(ElementName = "ChiTiet")]
    public class ChiTiet
    {
        [XmlElement(ElementName = "maHoaDon")] public string MaHoaDon { get; set; }

        [XmlElement(ElementName = "tenHDon")] public string TenHDon { get; set; }

        [XmlElement(ElementName = "kHieuMauHDon")]
        public string KHieuMauHDon { get; set; }

        [XmlElement(ElementName = "kHieuHDon")]
        public string KHieuHDon { get; set; }

        [XmlElement(ElementName = "soTonMuaTrKy_tongSo")]
        public string SoTonMuaTrKy_tongSo { get; set; }

        [XmlElement(ElementName = "soTonDauKy_tuSo")]
        public string SoTonDauKy_tuSo { get; set; }

        [XmlElement(ElementName = "soTonDauKy_denSo")]
        public string SoTonDauKy_denSo { get; set; }

        [XmlElement(ElementName = "muaTrongKy_tuSo")]
        public string MuaTrongKy_tuSo { get; set; }

        [XmlElement(ElementName = "muaTrongKy_denSo")]
        public string MuaTrongKy_denSo { get; set; }

        [XmlElement(ElementName = "tongSoSuDung_tuSo")]
        public string TongSoSuDung_tuSo { get; set; }

        [XmlElement(ElementName = "tongSoSuDung_denSo")]
        public string TongSoSuDung_denSo { get; set; }

        [XmlElement(ElementName = "tongSoSuDung_cong")]
        public string TongSoSuDung_cong { get; set; }

        [XmlElement(ElementName = "soDaSDung")]
        public string SoDaSDung { get; set; }

        [XmlElement(ElementName = "xoaBo_soLuong")]
        public string XoaBo_soLuong { get; set; }

        [XmlElement(ElementName = "xoaBo_so")] public string XoaBo_so { get; set; }

        [XmlElement(ElementName = "mat_soLuong")]
        public string Mat_soLuong { get; set; }

        [XmlElement(ElementName = "mat_so")] public string Mat_so { get; set; }

        [XmlElement(ElementName = "huy_soLuong")]
        public string Huy_soLuong { get; set; }

        [XmlElement(ElementName = "huy_so")] public string Huy_so { get; set; }

        [XmlElement(ElementName = "tonCuoiKy_tuSo")]
        public string TonCuoiKy_tuSo { get; set; }

        [XmlElement(ElementName = "tonCuoiKy_denSo")]
        public string TonCuoiKy_denSo { get; set; }

        [XmlElement(ElementName = "tonCuoiKy_soLuong")]
        public string TonCuoiKy_soLuong { get; set; }

        [XmlAttribute(AttributeName = "id")] public string Id { get; set; }
    }

    [XmlRoot(ElementName = "HoaDon")]
    public class HoaDon
    {
        [XmlElement(ElementName = "ChiTiet")] public List<ChiTiet> ChiTiet { get; set; }
    }

    [XmlRoot(ElementName = "CTieuTKhaiChinh")]
    public class CTieuTKhaiChinh
    {
        [XmlElement(ElementName = "kyBCaoCuoi")]
        public string KyBCaoCuoi { get; set; }

        [XmlElement(ElementName = "chuyenDiaDiem")]
        public string ChuyenDiaDiem { get; set; }

        [XmlElement(ElementName = "ngayDauKyBC")]
        public string NgayDauKyBC { get; set; }

        [XmlElement(ElementName = "ngayCuoiKyBC")]
        public string NgayCuoiKyBC { get; set; }

        [XmlElement(ElementName = "HoaDon")] public HoaDon HoaDon { get; set; }

        [XmlElement(ElementName = "tongCongSoTonDKy")]
        public string TongCongSoTonDKy { get; set; }

        [XmlElement(ElementName = "tongCongSDung")]
        public string TongCongSDung { get; set; }

        [XmlElement(ElementName = "tongCongSoTonCKy")]
        public string TongCongSoTonCKy { get; set; }

        [XmlElement(ElementName = "nguoiLapBieu")]
        public string NguoiLapBieu { get; set; }

        [XmlElement(ElementName = "nguoiDaiDien")]
        public string NguoiDaiDien { get; set; }

        [XmlElement(ElementName = "ngayBCao")] public string NgayBCao { get; set; }
    }

    [XmlRoot(ElementName = "HSoKhaiThue")]
    public class HSoKhaiThue
    {
        [XmlElement(ElementName = "TTinChung")]
        public TTinChung TTinChung { get; set; } = new TTinChung();

        [XmlElement(ElementName = "CTieuTKhaiChinh")]
        public CTieuTKhaiChinh CTieuTKhaiChinh { get; set; } = new CTieuTKhaiChinh();
    }

    [XmlRoot(ElementName = "HSoThueDTu")]
    public class HSoThueDTu
    {
        [XmlElement(ElementName = "HSoKhaiThue")]
        public HSoKhaiThue HSoKhaiThue { get; set; } = new HSoKhaiThue();
    }
}
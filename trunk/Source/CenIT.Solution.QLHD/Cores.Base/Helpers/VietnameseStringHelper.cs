using System.Text;

namespace Cores.Base.Helpers
{
    public class VietnameseStringHelper
    {
        public static string RemoveVietnameseAccents(string str)
        {
            if (str == null) return null;

            // Khai báo bảng chữ cái có dấu và không dấu
            var withAccentLower = "àáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ";
            var withAccentUpper = withAccentLower.ToUpper();
            var withoutAccent = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyd";

            // Loại bỏ dấu từng ký tự
            var sb = new StringBuilder();
            foreach (var ch in str)
            {
                var indexLower = withAccentLower.IndexOf(ch);
                var indexUpper = withAccentUpper.IndexOf(ch);

                if (indexLower != -1)
                    sb.Append(withoutAccent[indexLower]);
                else if (indexUpper != -1)
                    sb.Append(char.ToUpper(withoutAccent[indexUpper]));
                else
                    sb.Append(ch);
            }

            return sb.ToString();
        }
    }
}
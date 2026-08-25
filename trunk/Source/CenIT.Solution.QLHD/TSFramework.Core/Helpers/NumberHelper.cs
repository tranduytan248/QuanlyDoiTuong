using System;

namespace TSFramework.Core.Helpers
{
    public class NumberHelper
    {
        public static string NumberToString(string number)
        {
            var strReturn = "";
            var s = number;
            while (s.Length > 0 && s.Substring(0, 1) == "0") s = s.Substring(1);
            string[] so = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = { "", "nghìn", "triệu", "tỷ" };
            int i, j, unit, chuc, tram;

            var booAm = false;
            decimal decS = 0;

            try
            {
                decS = Convert.ToDecimal(s);
            }
            catch
            {
                // ignored
            }

            if (decS < 0)
                //s = decS.ToString();
                booAm = true;
            i = s.Length;
            if (i == 0)
            {
                strReturn = so[0] + strReturn;
            }
            else
            {
                j = 0;
                while (i > 0)
                {
                    unit = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if (unit > 0 || chuc > 0 || tram > 0 || j == 3)
                        strReturn = hang[j] + strReturn;
                    j++;
                    if (j > 3) j = 1; //Tránh lỗi, nếu dưới 13 số thì không có vấn đề.
                    //Hàm này chỉ dùng để đọc đến 9 số nên không phải bận tâm
                    if (unit == 1 && chuc > 1)
                    {
                        strReturn = "mốt " + strReturn;
                    }
                    else
                    {
                        if (unit == 5 && chuc > 0)
                            strReturn = "lăm " + strReturn;
                        else if (unit > 0)
                            strReturn = so[unit] + " " + strReturn;
                    }

                    if (chuc < 0) break; //Hết số
                    if (chuc == 0 && unit > 0) strReturn = "linh " + strReturn;
                    if (chuc == 1) strReturn = "mười " + strReturn;
                    if (chuc > 1) strReturn = so[chuc] + " mươi " + strReturn;
                    if (tram < 0) break; //Hết số
                    if (tram > 0 || chuc > 0 || unit > 0) strReturn = so[tram] + " trăm " + strReturn;
                    strReturn = " " + strReturn;
                }
            }

            if (booAm) strReturn = "Âm " + strReturn;
            var result = strReturn.Trim().Substring(0, 1).ToUpper() + strReturn.Trim().Substring(1) + " đồng";
            return result;
        }

        public static string NumberToCharacter(double n)
        {
            string[] numbersArr =
            {
                "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín", "mười", "mười một", "mười hai",
                "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín"
            };
            string[] tensArr =
                { "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
            string[] suffixesArr =
            {
                "nghìn", "triệu", "tỷ", "nghìn tỷ", "quadrillion", "quintillion", "sextillion", "septillion",
                "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion",
                "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion",
                "Novemdecillion", "Vigintillion"
            };
            var words = "";

            var tens = false;

            if (n < 0)
            {
                words += "negative ";
                n *= -1;
            }

            var power = (suffixesArr.Length + 1) * 3;

            while (power > 3)
            {
                var pow = Math.Pow(10, power);
                if (n >= pow)
                {
                    if (n % pow > 0)
                        words += NumberToCharacter(Math.Floor(n / pow)) + " " + suffixesArr[power / 3 - 1] + " ";
                    else if (n % pow == 0)
                        words += NumberToCharacter(Math.Floor(n / pow)) + " " + suffixesArr[power / 3 - 1];
                    n %= pow;
                }

                power -= 3;
            }

            if (n >= 1000)
            {
                if (n % 1000 > 0) words += NumberToCharacter(Math.Floor(n / 1000)) + " nghìn ";
                else words += NumberToCharacter(Math.Floor(n / 1000)) + " nghìn";
                n %= 1000;
            }

            if (!(0 <= n) || !(n <= 999)) return words;
            if ((int)n / 100 > 0)
            {
                words += NumberToCharacter(Math.Floor(n / 100)) + " trăm ";
                n %= 100;
            }

            if ((int)n / 10 > 1)
            {
                //if (words != "")
                //    words += " ";
                words += tensArr[(int)n / 10 - 2];
                tens = true;
                n %= 10;
            }

            if (!(n < 20) || !(n > 0)) return words;
            if (words != "" && !tens)
                words += " ";
            words += tens ? " " + numbersArr[(int)n - 1] : numbersArr[(int)n - 1];
            n -= Math.Floor(n);

            return words;
        }

        public static string NumberToVietnamese(decimal n)
        {
            var numberInput = (long)n;
            string[] numberName =
            {
                "không", //0
                "một", //1
                "hai", //2
                "ba", //3
                "bốn", //4
                "năm", //5
                "sáu", //6
                "bảy", //7
                "tám", //8
                "chín" //9
            };

            string[] groupNumberName =
            {
                "",
                "ngàn",
                "triệu",
                "tỷ",
                "nghìn tỷ",
                "triệu tỷ",
                "tỷ tỷ"
            };

            if (numberInput == 0) return "không";

            var isNegative = false;
            if (numberInput < 0)
            {
                numberInput = -numberInput;
                isNegative = true;
            }

            int unit = -1, tenth = -1, hundreds = -1;
            var groupIndex = 0;
            var strOutput = "";
            //loop through the } of str_number to the {ning
            while (numberInput > 0)
            {
                unit = (int)(numberInput % 10);
                numberInput = numberInput / 10;

                if (numberInput > 0)
                {
                    tenth = (int)(numberInput % 10);
                    numberInput = numberInput / 10;
                }
                else
                {
                    tenth = -1;
                }

                if (numberInput > 0)
                {
                    hundreds = (int)(numberInput % 10);
                    numberInput = numberInput / 10;
                }
                else
                {
                    hundreds = -1;
                }

                //three digits make a group
                if (unit > 0 || tenth > 0 || hundreds > 0 || groupIndex == 3)
                    strOutput = groupNumberName[groupIndex] + strOutput;
                groupIndex = groupIndex + 1;

                if (groupIndex > 3)
                    groupIndex = 1;

                if (unit == 1 && tenth > 1)
                    strOutput = "một " + strOutput;
                else if (unit == 5 && tenth > 0)
                    strOutput = "lăm " + strOutput;
                else if (unit > 0)
                    strOutput = numberName[unit] + " " + strOutput;

                if (tenth < 0) break;

                if (tenth == 0 && unit > 0)
                    strOutput = "lẻ " + strOutput;
                else if (tenth == 1)
                    strOutput = "mười " + strOutput;
                else if (tenth > 1)
                    strOutput = numberName[tenth] + " mươi " + strOutput;

                if (hundreds < 0) break;

                if (hundreds > 0 || tenth > 0 || unit > 0)
                    strOutput = " " + numberName[hundreds] + " trăm " + strOutput;
                //While }
            }

            //change the call of "1" after tenth
            strOutput = strOutput.Replace("mươi một", "mươi mốt");

            if (isNegative)
                strOutput = "âm " + strOutput;
            return strOutput.Trim();
        }

        public static string UpperCaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s)) return string.Empty;
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static double ReverseCalcTax(double totalAmount, float taxRate)
        {
            //totalAmount = ((taxRate + 100) * amountWithoutTax) / 100;
            return Math.Round(totalAmount * 100 / (taxRate + 100), 0);
        }
    }
}
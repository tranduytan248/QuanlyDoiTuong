using System.ComponentModel.DataAnnotations;
using TSFramework.App.Processors;

namespace TSFramework.App.Attributes
{
    public class CustomMaxLengthAttribute : StringLengthAttribute
    {
        public CustomMaxLengthAttribute(int length) : base(length)
        {
            //ErrorMessage = "{0} length should not be more than {2}";
            ErrorMessage = AppProcessor.Messagor.GetMessage("Common_MaxLengthMessage");
        }
    }
}
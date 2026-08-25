using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace TSFramework.App.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AliasAttribute : Attribute
    {
        public AliasAttribute(string name)
        {
            Name = name;
        }

        protected internal string Name { get; set; }
    }

    public class AliasBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            var alias = propertyDescriptor.Attributes
                .OfType<AliasAttribute>()
                .FirstOrDefault();

            var text = CreateSubPropertyName(bindingContext.ModelName, alias?.Name ?? propertyDescriptor.Name);
            if (!bindingContext.ValueProvider.ContainsPrefix(text)) return;
            var binder = Binders.GetBinder(propertyDescriptor.PropertyType);
            var value = propertyDescriptor.GetValue(bindingContext.Model);
            var modelMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
            modelMetadata.Model = value;
            var bindingContext2 = new ModelBindingContext
            {
                ModelMetadata = modelMetadata,
                ModelName = text,
                ModelState = bindingContext.ModelState,
                ValueProvider = bindingContext.ValueProvider
            };
            var propertyValue = GetPropertyValue(controllerContext, bindingContext2, propertyDescriptor, binder);
            modelMetadata.Model = propertyValue;
            var modelState = bindingContext.ModelState[text];

            if (modelState == null || modelState.Errors.Count == 0)
            {
                if (OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, propertyValue))
                {
                    SetProperty(controllerContext, bindingContext, propertyDescriptor, propertyValue);
                    OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, propertyValue);
                }
            }
            else
            {
                SetProperty(controllerContext, bindingContext, propertyDescriptor, propertyValue);
                foreach (var current in (from err in modelState.Errors
                             where string.IsNullOrEmpty(err.ErrorMessage) && err.Exception != null
                             select err).ToList())
                    for (var ex = current.Exception; ex != null; ex = ex.InnerException)
                        if (ex is FormatException)
                        {
                            var displayName = modelMetadata.GetDisplayName();
                            var errorMessage = string.Format(CultureInfo.CurrentCulture,
                                $"'{0}' is not valid for field {1}.", modelState.Value.AttemptedValue, displayName);
                            modelState.Errors.Remove(current);
                            modelState.Errors.Add(errorMessage);
                            break;
                        }
            }
        }
    }
}
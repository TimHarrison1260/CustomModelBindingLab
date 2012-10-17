using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomModelBindingDemo.Models;

namespace CustomModelBindingDemo.Infrastructure
{
    // see http://ivanz.com/2010/11/03/custom-model-binding-using-imodelbinder-in-asp-net-mvc-two-gotchas/ 
    // for an example with value type members

    public class MemberModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, 
            ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext", "controllerContext is null.");
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext", "bindingContext is null.");

            string firstName = TryGet<string>(bindingContext, "FirstName");
            string lastName = TryGet<string>(bindingContext, "LastName");

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                return new Member(firstName, lastName);

            return null;
        }

        private T TryGet<T>(ModelBindingContext bindingContext, string key) where T : class
        {
            if (String.IsNullOrEmpty(key))
                return null;

            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix == true)
                valueResult = bindingContext.ValueProvider.GetValue(key);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            if (valueResult == null)
                return null;

            try
            {
                return (T)valueResult.ConvertTo(typeof(T));
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
                return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;           // for the defaultmodelbinder class
using CustomModelBindingDemo.Models;    //  For the models

namespace CustomModelBindingDemo.Infrastructure
{
    public class DateOfBirthModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, 
            ModelBindingContext bindingContext, 
            System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            //  intercept the DateOfBirth property to construct its values from
            //  the Request form values.
            if (propertyDescriptor.PropertyType == typeof(DateTime?) && propertyDescriptor.Name == "DateOfBirth")
            {
                //  Controller context provides access to the HTTPContext
                //  Binding context provides access to the ValueProviders

                //  We want to get the value passed in with the request so we use the ControllerContext object
                var request = controllerContext.HttpContext.Request;
                //  Get the birthDay, birthMonth and birthYear values from the request
                string day = request["birthDay"];
                string month = request["birthMonth"];
                string year = request["birthYear"];
                string strDoB = string.Format("{0}/{1}/{2}", day, month, year);

                DateTime DoB;
                if (DateTime.TryParse(strDoB, out DoB))
                {
                    //  If we have a date, set the property of the DateOfBirth.
                    SetProperty(controllerContext, bindingContext, propertyDescriptor, DoB);
                    return;
                }
                else
                {
                    //  We have not got a date so set a ModelStateError
                    bindingContext.ModelState.AddModelError("DateOfBirth", "Invalid date supplied, please enter a valid date");
                    return;
                }
            }
            //  If we get here, we're not processing a DateOfBirth, so use the defaultModelBinder.
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}
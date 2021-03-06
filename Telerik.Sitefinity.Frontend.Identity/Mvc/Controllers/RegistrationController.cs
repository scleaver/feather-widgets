﻿using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Telerik.Sitefinity.Frontend.Identity.Mvc.Models.Registration;
using Telerik.Sitefinity.Frontend.Identity.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Mvc;

namespace Telerik.Sitefinity.Frontend.Identity.Mvc.Controllers
{
    /// <summary>
    /// This class represents the controller of the Registration widget.
    /// </summary>
    [Localization(typeof(RegistrationResources))]
    [ControllerToolboxItem(Name = "Registration_MVC", Title = "Registration", SectionName = "Users", CssClass = RegistrationController.WidgetIconCssClass)]
    public class RegistrationController : Controller
    {
        #region Properties

        /// <summary>
        /// Gets the Registration widget model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual IRegistrationModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = this.InitializeModel();

                return this.model;
            }
        }

        /// <summary>
        /// Gets or sets if on registration the email addess should be used as the username
        /// </summary>
        public bool EmailAddressShouldBeTheUsername {
            get {
                return this.emailAddressShouldBeTheUsername;
            }

            set {
                this.emailAddressShouldBeTheUsername = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the template that widget will be displayed.
        /// </summary>
        /// <value></value>
        public string TemplateName
        {
            get
            {
                return this.templateName;
            }

            set
            {
                this.templateName = value;
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Renders appropriate list view depending on the <see cref="TemplateName" />
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult" />.
        /// </returns>
        public ActionResult Index()
        {
            var fullTemplateName = this.templateNamePrefix + this.TemplateName;

            var viewModel = new RegistrationViewModel();
            viewModel.EmailAddressShouldBeTheUsername = this.EmailAddressShouldBeTheUsername;

            this.Model.InitializeViewModel(viewModel);

            this.ViewBag.ShowSuccessfulRegistrationMsg = false;
            this.ViewBag.Error = this.Model.GetError();

            return this.View(fullTemplateName, viewModel);
        }

        /// <summary>
        /// Posts the registration form.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        /// The <see cref="ActionResult" />.
        /// </returns>
        [HttpPost]
        public ActionResult Index(RegistrationViewModel viewModel)
        {
            viewModel.EmailAddressShouldBeTheUsername = this.EmailAddressShouldBeTheUsername;

            if(this.EmailAddressShouldBeTheUsername){
                ModelState.Remove("UserName");
            }

            if (ModelState.IsValid)
            {
                var status = this.Model.RegisterUser(viewModel);
                if (status == MembershipCreateStatus.Success)
                {
                    if (this.Model.ActivationMethod == ActivationMethod.AfterConfirmation)
                    {
                        this.ViewBag.ShowActivationMsg = true;
                    }
                    if (this.Model.SuccessfulRegistrationAction == SuccessfulRegistrationAction.ShowMessage)
                    {
                        this.ViewBag.ShowSuccessfulRegistrationMsg = true;
                    }
                    else if (this.Model.SuccessfulRegistrationAction == SuccessfulRegistrationAction.RedirectToPage)
                    {
                        return this.Redirect(this.Model.GetPageUrl(this.Model.SuccessfulRegistrationPageId));
                    }
                }
                else
                {
                    this.ViewBag.Error = this.Model.ErrorMessage(status);
                }
            }

            this.Model.InitializeViewModel(viewModel);

            var fullTemplateName = this.templateNamePrefix + this.TemplateName;
            return this.View(fullTemplateName, viewModel);
        }

        /// <summary>
        /// Resends the confirmation email.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public JsonResult ResendConfirmationEmail(string email)
        {
            var isSend = this.Model.ResendConfirmationEmail(email);

            return Json(isSend, JsonRequestBehavior.AllowGet);
        }

        /// <inheritDocs/>
        protected override void HandleUnknownAction(string actionName)
        {
            this.Index().ExecuteResult(this.ControllerContext);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the model.
        /// </summary>
        /// <returns>
        /// The <see cref="IRegistrationModel"/>.
        /// </returns>
        private IRegistrationModel InitializeModel()
        {
            return ControllerModelFactory.GetModel<IRegistrationModel>(this.GetType());
        }

        #endregion

        #region Private fields and constants

        internal const string WidgetIconCssClass = "sfCreateAccountIcn sfMvcIcn";

        private string templateName = "RegistrationForm";
        private IRegistrationModel model;
        private string templateNamePrefix = "Registration.";
        private bool emailAddressShouldBeTheUsername = false;

        #endregion
    }
}

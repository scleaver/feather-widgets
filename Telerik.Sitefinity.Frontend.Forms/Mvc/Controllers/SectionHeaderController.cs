﻿using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.SectionHeader;
using Telerik.Sitefinity.Frontend.Forms.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages.Web.Services;
using Telerik.Sitefinity.Web.UI.Fields.Enums;

namespace Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers
{
    /// <summary>
    /// This class represents the controller of the MVC forms section header field.
    /// </summary>
    [DatabaseMapping(UserFriendlyDataType.ShortText)]
    [Localization(typeof(FieldResources))]
    public class SectionHeaderController : FormElementControllerBase<ISectionHeaderModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionHeaderController"/> class.
        /// </summary>
        public SectionHeaderController()
        {
            this.DisplayMode = FieldDisplayMode.Read;
            this.ReadTemplateName = SectionHeaderController.TemplateName;
        }

        /// <inheritDocs />
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [ReflectInheritedProperties]
        public override ISectionHeaderModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = ControllerModelFactory.GetModel<ISectionHeaderModel>(this.GetType());

                return this.model;
            }
        }

        /// <inheritDocs />
        protected override string ReadTemplateNamePrefix
        {
            get
            {
                return SectionHeaderController.TemplateNamePrefix;
            }
        }

        /// <summary>
        /// This form element doens't support write mode so redirect to read.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override ActionResult Write(object value)
        {
            return this.Read(value);
        }

        /// <inheritDocs />
        protected override void HandleUnknownAction(string actionName)
        {
            this.Read(null).ExecuteResult(this.ControllerContext);
        }

        private ISectionHeaderModel model;
        private const string TemplateNamePrefix = "Read.";
        private const string TemplateName = "Default";
    }
}
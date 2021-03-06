// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Programmatic.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Properties for creating a new LUIS Application
    /// </summary>
    public partial class ApplicationCreateObject
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationCreateObject class.
        /// </summary>
        public ApplicationCreateObject()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationCreateObject class.
        /// </summary>
        /// <param name="culture">The culture for the new application. It is
        /// the language that your app understands and speaks. E.g.: "en-us".
        /// Note: the culture cannot be changed after the app is
        /// created.</param>
        /// <param name="name">The name for the new application.</param>
        /// <param name="domain">The domain for the new application. Optional.
        /// E.g.: Comics.</param>
        /// <param name="description">Description of the new application.
        /// Optional.</param>
        /// <param name="initialVersionId">The initial version ID. Optional.
        /// Default value is: "0.1"</param>
        /// <param name="usageScenario">Defines the scenario for the new
        /// application. Optional. E.g.: IoT.</param>
        public ApplicationCreateObject(string culture, string name, string domain = default(string), string description = default(string), string initialVersionId = default(string), string usageScenario = default(string))
        {
            Culture = culture;
            Domain = domain;
            Description = description;
            InitialVersionId = initialVersionId;
            UsageScenario = usageScenario;
            Name = name;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the culture for the new application. It is the
        /// language that your app understands and speaks. E.g.: "en-us". Note:
        /// the culture cannot be changed after the app is created.
        /// </summary>
        [JsonProperty(PropertyName = "culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the domain for the new application. Optional. E.g.:
        /// Comics.
        /// </summary>
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets description of the new application. Optional.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the initial version ID. Optional. Default value is:
        /// "0.1"
        /// </summary>
        [JsonProperty(PropertyName = "initialVersionId")]
        public string InitialVersionId { get; set; }

        /// <summary>
        /// Gets or sets defines the scenario for the new application.
        /// Optional. E.g.: IoT.
        /// </summary>
        [JsonProperty(PropertyName = "usageScenario")]
        public string UsageScenario { get; set; }

        /// <summary>
        /// Gets or sets the name for the new application.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Culture == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Culture");
            }
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
        }
    }
}

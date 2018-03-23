/* 
 * My API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = IO.Swagger.Client.SwaggerDateConverter;

namespace IO.Swagger.Model
{
    /// <summary>
    /// Defines the structure of a Department when serializing and deserializing data. Data transfer objects (DTOs)   were introduced in the project due to problems with circular references in the model classes.
    /// </summary>
    [DataContract]
    public partial class DepartmentDTO :  IEquatable<DepartmentDTO>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentDTO" /> class.
        /// </summary>
        /// <param name="Id">The id of the department..</param>
        /// <param name="Name">The name of the department..</param>
        /// <param name="Members">A list of the ids of all members of the department..</param>
        /// <param name="Resources">A list of ids of all resources owned by the department..</param>
        public DepartmentDTO(long? Id = default(long?), string Name = default(string), List<string> Members = default(List<string>), List<long?> Resources = default(List<long?>))
        {
            this.Id = Id;
            this.Name = Name;
            this.Members = Members;
            this.Resources = Resources;
        }
        
        /// <summary>
        /// The id of the department.
        /// </summary>
        /// <value>The id of the department.</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public long? Id { get; set; }

        /// <summary>
        /// The name of the department.
        /// </summary>
        /// <value>The name of the department.</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// A list of the ids of all members of the department.
        /// </summary>
        /// <value>A list of the ids of all members of the department.</value>
        [DataMember(Name="members", EmitDefaultValue=false)]
        public List<string> Members { get; set; }

        /// <summary>
        /// A list of ids of all resources owned by the department.
        /// </summary>
        /// <value>A list of ids of all resources owned by the department.</value>
        [DataMember(Name="resources", EmitDefaultValue=false)]
        public List<long?> Resources { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DepartmentDTO {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Members: ").Append(Members).Append("\n");
            sb.Append("  Resources: ").Append(Resources).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as DepartmentDTO);
        }

        /// <summary>
        /// Returns true if DepartmentDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of DepartmentDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DepartmentDTO input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Members == input.Members ||
                    this.Members != null &&
                    this.Members.SequenceEqual(input.Members)
                ) && 
                (
                    this.Resources == input.Resources ||
                    this.Resources != null &&
                    this.Resources.SequenceEqual(input.Resources)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Members != null)
                    hashCode = hashCode * 59 + this.Members.GetHashCode();
                if (this.Resources != null)
                    hashCode = hashCode * 59 + this.Resources.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}

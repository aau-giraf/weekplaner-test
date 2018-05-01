/* 
 * The Giraf REST API
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
    /// Defines the structure of Weekday when serializing and deserializing data. Data transfer objects (DTOs)   were introduced in the project due to problems with circular references in the model classes.
    /// </summary>
    [DataContract]
    public partial class WeekdayDTO :  IEquatable<WeekdayDTO>, IValidatableObject
    {
        /// <summary>
        /// An enum defining which day of the week this Weekday represents.
        /// </summary>
        /// <value>An enum defining which day of the week this Weekday represents.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DayEnum
        {
            
            /// <summary>
            /// Enum Monday for value: Monday
            /// </summary>
            [EnumMember(Value = "Monday")]
            Monday = 1,
            
            /// <summary>
            /// Enum Tuesday for value: Tuesday
            /// </summary>
            [EnumMember(Value = "Tuesday")]
            Tuesday = 2,
            
            /// <summary>
            /// Enum Wednesday for value: Wednesday
            /// </summary>
            [EnumMember(Value = "Wednesday")]
            Wednesday = 3,
            
            /// <summary>
            /// Enum Thursday for value: Thursday
            /// </summary>
            [EnumMember(Value = "Thursday")]
            Thursday = 4,
            
            /// <summary>
            /// Enum Friday for value: Friday
            /// </summary>
            [EnumMember(Value = "Friday")]
            Friday = 5,
            
            /// <summary>
            /// Enum Saturday for value: Saturday
            /// </summary>
            [EnumMember(Value = "Saturday")]
            Saturday = 6,
            
            /// <summary>
            /// Enum Sunday for value: Sunday
            /// </summary>
            [EnumMember(Value = "Sunday")]
            Sunday = 7
        }

        /// <summary>
        /// An enum defining which day of the week this Weekday represents.
        /// </summary>
        /// <value>An enum defining which day of the week this Weekday represents.</value>
        [DataMember(Name="day", EmitDefaultValue=false)]
        public DayEnum? Day { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="WeekdayDTO" /> class.
        /// </summary>
        /// <param name="Day">An enum defining which day of the week this Weekday represents..</param>
        /// <param name="Activities">A list of all the activities of the week..</param>
        public WeekdayDTO(DayEnum? Day = default(DayEnum?), List<ActivityDTO> Activities = default(List<ActivityDTO>))
        {
            this.Day = Day;
            this.Activities = Activities;
        }
        

        /// <summary>
        /// A list of all the activities of the week.
        /// </summary>
        /// <value>A list of all the activities of the week.</value>
        [DataMember(Name="activities", EmitDefaultValue=false)]
        public List<ActivityDTO> Activities { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WeekdayDTO {\n");
            sb.Append("  Day: ").Append(Day).Append("\n");
            sb.Append("  Activities: ").Append(Activities).Append("\n");
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
            return this.Equals(input as WeekdayDTO);
        }

        /// <summary>
        /// Returns true if WeekdayDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of WeekdayDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WeekdayDTO input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Day == input.Day ||
                    (this.Day != null &&
                    this.Day.Equals(input.Day))
                ) && 
                (
                    this.Activities == input.Activities ||
                    this.Activities != null &&
                    this.Activities.SequenceEqual(input.Activities)
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
                if (this.Day != null)
                    hashCode = hashCode * 59 + this.Day.GetHashCode();
                if (this.Activities != null)
                    hashCode = hashCode * 59 + this.Activities.GetHashCode();
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

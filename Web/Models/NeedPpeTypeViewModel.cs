using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Web.Db;
using Web.Infrastructure;
using Web.Snippets.System.ComponentModel.DataAnnotations;

namespace Web.Models
{
	public partial class NeedPpeTypeViewModel : IValidatableObject
	{
		public PpeTypes Type { get; set; }
		public bool Selected { get; set; }

		[Display(Name = "PPE Type Other", Description = "If the list above does not fit choose \"Other...\" and describe here"),
		StringLength(1000, MinimumLength = 3, ErrorMessage = Settings.ValMsgs.StringLengthWithMinimum)]
		public string PpeTypeOther { get; set; }

		[Display(Name = "Daily Shortage"),
		NumericalRange(1, 1, 1000000)]
		public int? DailyShortage { get; set; }

		[Display(Name = "Daily FFP1 Shortage Details", Description = "Please tell us who the daily number above applies to. For example:</p><p class='description'>Entire ICU Ward</p><p class='description'>Team of 15 working nights"),
		StringLength(500, MinimumLength = 3, ErrorMessage = Settings.ValMsgs.StringLengthWithMinimum)]
		public string DailyShortageForWhom { get; set; }

		public string DailyShortageDisplayName
		{
			get
			{
				return Type switch {
					PpeTypes.FFP1RespiratorMasks => "Daily FFP1 Shortage",
					_ => "", // Description only
				};
			}
		}

		public string DailyShortageDisplayDescription
		{
			get
			{
				return Type switch {
					PpeTypes.FFP1RespiratorMasks => "Estimate how many are missing per day</p><p class='description'>We are working with our not-for-profit partners <a href='https://www.caregiven.co.uk/'>Care Given</a> to get FFP1 Masks to those in need",
					_ => "We are a charity. Our supplies are limited. How many do you (and your team) really need to bridge the gap until supplies arrive?",
				};
			}
		}

		public string DailyShortageForWhomDisplayName
		{
			get
			{
				return Type switch {
					PpeTypes.FFP1RespiratorMasks => "Daily FFP1 Shortage Details",
					_ => "", // Discription only
				};
			}
		}

		public string DailyShortageForWhomDisplayDescription
		{
			get
			{
				return Type switch {
					PpeTypes.FFP1RespiratorMasks => "Please tell us who the daily number above applies to. For example:</p><p class='description'>Entire ICU Ward</p><p class='description'>Team of 15 working nights",
					_ => "How many people will this protect for how long?",
				};
			}
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> respVal = new List<ValidationResult>();
			if(Type == PpeTypes.Other && Selected && String.IsNullOrWhiteSpace(PpeTypeOther))
			{
				respVal.Add(new ValidationResult("Please add <b>PPE Type Other</b> to describe the PPE Type when choosing <b>\"Other...\"</b>", 
					new List<string> { $"{PpeTypeOther}" }));
			}
			if (Selected)
			{
				if(DailyShortage == null)
				{
					respVal.Add(new ValidationResult($"Please add <b>{DailyShortageDisplayName}</b>", 
						new List<string> { $"{nameof(DailyShortage)}" }));
				}
				else
				{
					MemberInfo m = validationContext.ObjectType.GetMember(nameof(DailyShortage))[0];
					NumericalRangeAttribute numRange = m.GetCustomAttributes(typeof(NumericalRangeAttribute), false).Cast<NumericalRangeAttribute>().Single();
					DisplayAttribute displayAttribute = m.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single();
					if(!(numRange.Min <= DailyShortage.Value && DailyShortage.Value <= numRange.Max))
					{
						respVal.Add(new ValidationResult(String.Format(Settings.ValMsgs.NumRange, displayAttribute.Name, numRange.Min, numRange.Max), 
							new List<string> { $"{nameof(DailyShortage)}" }));
					}
				}
			}
			if(Type == PpeTypes.FFP1RespiratorMasks && Selected)
			{
				if(String.IsNullOrWhiteSpace(DailyShortageForWhom))
				{
					respVal.Add(new ValidationResult("Please add <b>Daily FFP1 Shortage Details</b>", 
						new List<string> { $"{nameof(DailyShortageForWhom)}" }));
				}
			}
			return respVal;
		}

		public static NeedPpeTypeViewModel FromPpeType(PpeTypes type) { return new NeedPpeTypeViewModel { Type = type }; }
	}
}
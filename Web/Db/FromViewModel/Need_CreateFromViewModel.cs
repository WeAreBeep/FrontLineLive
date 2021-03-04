using System;
using System.Linq;
using Web.Infrastructure;
using Web.Models;

namespace Web.Db
{
	public partial class Need
	{
		/// <summary>
		/// For Inserts
		/// </summary>
		public static Need CreateFromViewModel(NeedsViewModel s)
		{
			Need respVal = new Need
			{
				PublishAnonymously = s.PublishAnonymously,
				Timestamp = DateTimeOffset.Now, //N.B for inserts 
				StatusId = 	(int)PostStatus.UnderReview,//N.B for inserts 
				ContactName = s.ContactName ?? "",
				JobTitle = s.JobTitle,
				Email = s.Email,
				PhoneNumber = s.PhoneNumber,
				OrganisationName = s.OrganisationName,
				Department = s.Department,
				OrgTypeId = (int)s.OrgType,
				OrgTypeOther = s.OrgTypeOther,
				TownOrCity = s.TownOrCity,
				Postcode = s.Postcode,
				TellUsMore = s.TellUsMore,
			};
			respVal.NeedPpeTypes = s.PpeTypes.Where(nt => nt.Selected).Select(nt => NeedPpeType.Create_FromViewModel(nt, respVal)).ToList();
			return respVal;
		}

		/// <summary>
		/// Very rough and ready guess at if a needs post OP is contactable
		/// Could easily lead to both false postivies and false negatives 
		/// //TODO:suggest bool checkbox on Admin / Volunteers Edit Needs Page,
		/// so they can tick Is/IsNot contactable then change Home Index Action to use that in where clause when vols. have updated all data 
		/// Believe the exisiting workaround in place is that they mark all PPETypes as NotMet to mark that it's really Not Meetable / Not Contactable
		/// </summary>
		public bool IsContactable(){
			bool respVal = false;
			if((!String.IsNullOrWhiteSpace(Email) && Email.Length >= _emailLengthSensible && Email != Settings.DefaultEmail)
			|| (!String.IsNullOrWhiteSpace(PhoneNumber) && PhoneNumber.Length >= _phoneLengthSensible)){
				respVal = true;
			}
			return respVal;
		}

		public bool IsMarkedAllNotMet()
		{
			return NeedPpeTypes.All(p => p.StatusId == (int) PpeStatus.NotMet);
		}

		/// <summary>
		/// Simply combines "vol workaround" with contactable best guess 
		/// => giving a lower number for Not Met on first chart, so bumping up the percentages on others
		/// Quite justifiable considering the history of the system 
		/// i.e. Ushahidi was for a long time not capturing contact details (for first X hundred needs posts) 
		/// => means vols. cannot meet those old needs
		/// </summary>
		public bool IsNotMarkedAllNotMetAndIsContactable()
		{
			return !IsMarkedAllNotMet() && IsContactable();
		}
		static int _emailLengthSensible => 7;
		static int _phoneLengthSensible => 6;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Web.Models;
using Web.Snippets.System.Collections.Generic;

namespace Web.Db
{
	public partial class Need
	{
		public void Modify(EditNeedsPost s, string currentUserId)
		{
			Modify(s.Request);
			StatusId = (int)s.Status.Status;
			TweetId = s.Twitter.TweetId;

			updateBasicPpeDetails(s);
			updatePpeMatchingDetails(s);

			Latitude = s.Location.Latitude;
			Longitude = s.Location.Longitude;

			if(!String.IsNullOrWhiteSpace(s.Notes.NewNote))
			{
				NeedNote newNote = new NeedNote
				{
					Need = this,
					Note = new Note
					{
						Timestamp = DateTimeOffset.Now,
						UserId = currentUserId,
						Text = s.Notes.NewNote,
					}
				};
				NeedNotes.Add(newNote);
			}
		}

		/// <summary>
		/// Handle the Needs Matching data ON RIGHT (i.e. Admin details such as PPE Status and Suppliers etc)
		/// mod those that are in post, and already exist
		/// </summary>
		void updatePpeMatchingDetails(EditNeedsPost s)
		{
			foreach(NeedsMatchData needsMatchData in s.NeedsMatching)
			{
				NeedPpeType needExisting = NeedPpeTypes.SingleOrDefault(p => p.PpeTypeId == (byte)needsMatchData.PpeType);
				needExisting?.Modify(needsMatchData);
			}
		}

		/// <summary>
		/// Handle the PPE Types on LEFT (i.e. basic details HWCs submit)
		/// </summary>
		void updateBasicPpeDetails(EditNeedsPost s)
		{
			List<PpeTypes> existingPpeTypes = NeedPpeTypes.SelectToList(p => (PpeTypes)p.PpeTypeId);

			//create new 
			List<NeedPpeTypeViewModel> ppeTypesToAdd = s.Request.PpeTypes.WhereToList(p => p.Selected && !existingPpeTypes.Contains(p.Type));
			foreach(NeedPpeTypeViewModel needPpeTypeViewModel in ppeTypesToAdd)
			{
				NeedPpeType ppeTypeToAdd = NeedPpeType.Create_FromViewModel(needPpeTypeViewModel, this);
				NeedPpeTypes.Add(ppeTypeToAdd);
			}

			//remove deselected 
			List<NeedPpeTypeViewModel> ppeTypesToRemove = s.Request.PpeTypes.WhereToList(p => !p.Selected && existingPpeTypes.Contains(p.Type));
			foreach(NeedPpeTypeViewModel needPpeTypeViewModel in ppeTypesToRemove)
			{
				NeedPpeType typeToRemove = NeedPpeTypes.Single(p => p.PpeTypeId == (byte)needPpeTypeViewModel.Type);
				NeedPpeTypes.Remove(typeToRemove);
			}

			//edit rest (existing) 
			List<NeedPpeTypeViewModel> ppeTypesToModify = s.Request.PpeTypes.WhereToList(p => p.Selected && existingPpeTypes.Contains(p.Type));
			foreach(NeedPpeTypeViewModel needPpeTypeViewModel in ppeTypesToModify)
			{
				NeedPpeType needExisting = NeedPpeTypes.SingleOrDefault(p => p.PpeTypeId == (byte)needPpeTypeViewModel.Type);
				needExisting?.Modify(needPpeTypeViewModel); //works??
			}
		}

		public void Modify(NeedsViewModel s)
		{
			PublishAnonymously = s.PublishAnonymously;
			ContactName = s.ContactName ?? "";
			JobTitle = s.JobTitle;
			Email = s.Email;
			PhoneNumber = s.PhoneNumber;
			OrganisationName = s.OrganisationName;
			Department = s.Department;
			OrgTypeId = (int)s.OrgType;
			OrgTypeOther = s.OrgTypeOther;
			TownOrCity = s.TownOrCity;
			Postcode = s.Postcode;
			TellUsMore = s.TellUsMore;
		}
	}
}
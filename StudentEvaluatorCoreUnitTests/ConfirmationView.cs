using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.View;

namespace StudentEvaluatorCoreUnitTests
{
	[ExcludeFromCodeCoverage]
	public class ConfirmationView : IConfirmationView
	{
		/// <summary>
		/// Gets or sets the confirmation result to return automatically from ConfirmAction.
		/// </summary>
		public ConfirmationResult RequestedConfirmation { get; set; }
		
		public ConfirmationResult ConfirmAction(ConfirmationOptions options, string caption, string message)
		{
			Debug.WriteLine("CONFIRMATION REQUEST:  {1} [{0}] ({2})", Enum.GetName(options.GetType(), options), caption, message);
			Debug.WriteLine("CONFIRMATION RESPONSE: {0})", Enum.GetName(RequestedConfirmation.GetType(), RequestedConfirmation));
			return this.RequestedConfirmation;
		}
	}
}

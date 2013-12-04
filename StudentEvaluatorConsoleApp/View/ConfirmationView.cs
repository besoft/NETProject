using System;

namespace Zcu.StudentEvaluator.View
{
	public class ConfirmationView : IConfirmationView
	{
		#region IConfirmationView
		/// <summary>
		/// Confirms the action to be done.
		/// </summary>
		/// <param name="options">Options available during the confirmation.</param>
		/// <param name="caption">The caption, i.e., a short summary of what is needed to be confirmed.</param>
		/// <param name="message">The detailed explanation of what is to be confirmed.</param>
		/// <returns>
		/// User decision.
		/// </returns>
		public ConfirmationResult ConfirmAction(ConfirmationOptions options, string caption, string message)
		{
			Console.WriteLine("----->\n{0}\n\n{1}\n", caption, message);
			while (true)
			{
				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Abort))
					Console.Write("(A)bort ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Retry))
					Console.Write("(R)etry ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Ignore))
					Console.Write("(I)gnore ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.OK))
					Console.Write("(O)K ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Yes))
					Console.Write("(Y)es ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.YesToAll))
					Console.Write("YesTo(A)ll ");	//Abort is not used with YesToAll

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.No))
					Console.Write("(N)o ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.NoToAll))
					Console.Write("No(T)oAll ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Cancel))
					Console.Write("(C)ancel ");

				Console.WriteLine();
				switch (Char.ToUpper(Console.ReadKey(true).KeyChar))
				{
					case 'A':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Abort))
							return ConfirmationResult.Abort;
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.YesToAll))
							return ConfirmationResult.YesToAll;
						break;

					case 'R':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Retry))
							return ConfirmationResult.Retry;
						break;

					case 'I':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Ignore))
							return ConfirmationResult.Ignore;
						break;

					case 'O':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.OK))
							return ConfirmationResult.OK;
						break;

					case 'Y':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Yes))
							return ConfirmationResult.Yes;
						break;

					case 'N':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.No))
							return ConfirmationResult.No;
						break;

					case 'T':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.NoToAll))
							return ConfirmationResult.NoToAll;
						break;

					case 'C':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Cancel))
							return ConfirmationResult.Cancel;
						break;
				}
			}
		}
		#endregion		
	}
}

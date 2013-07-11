#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

#endregion

namespace LmUtils
{
	public class StringUtilities
	{
		public static string FormatFileSize (ulong size)
		{
			string[] dims = new string[]
				{
					"КБ", "МБ", "ГБ", "ТБ"
				};

			if (size < 1024)
				return String.Format ("{0} байт", size);
			else
			{
				int k;
				for (k = 0; k < dims.Length-1; k++)
				{
					if (size < Math.Pow (1024, k + 2))
						return String.Format ("{0:.##} {1}", size / Math.Pow (1024, k + 1), dims[k]);
				}

				return String.Format ("{0:.##} {1}", size / Math.Pow (1024, k + 1), dims[k]);
			}
		}

		public class PathClassIdentity
		{
			public string name;
			public int index;

			public PathClassIdentity (string name, int index)
			{
				this.name = name;
				this.index = index;				
			}
		}

		public static Collection<PathClassIdentity> ParseClassPath (string path)
		{
			Collection<PathClassIdentity> parts = new Collection<PathClassIdentity> ();

			path = path.Replace (" ", "");
			string[] path_parts = path.Split ('.');

			if (path_parts.Length < 1)
				throw new ApplicationException (String.Format ("Invalid class path: '{0}'", path));

			foreach (string path_part in path_parts)
			{
				string name = path_part;
				int index = 0;

				Match match = Regex.Match (path_part,
					@"
							[^\w]*
							(?'name'[\w]+)
							\[  (?'index'[\d]+)   \]
							.*
						",
					RegexOptions.IgnorePatternWhitespace);
				
				if (match.Success && match.Groups.Count == 3)
				{
					try
					{
						name = match.Groups["name"].Value;
						index = Convert.ToInt32 (match.Groups["index"].Value);
					}
					catch
					{
					}
				}

				if (Regex.IsMatch (name, @"[^\w\d_]+") || name.Length < 1)
				{
					if (path_part == path_parts[path_parts.Length - 1] && name.EndsWith ("[]"))
					{
						name = name.Remove (name.LastIndexOf ("[]"));
						if (Regex.IsMatch (name, @"[^\w\d_]+"))
						{
							throw new ApplicationException (String.Format ("Invalid class path: '{0}'\nname: '{1}'", path, name));
						}
						else
						{
							parts.Add (new PathClassIdentity (name, -1));
						}
					}
					else
					{
						throw new ApplicationException (String.Format ("Invalid class path: '{0}'\nname: '{1}'", path, name));
					}
				}
				else
				{
					parts.Add (new PathClassIdentity (name, index));
				}
			}

			return parts;
		}

		/// <summary>
		/// Encodes string using triple-DES algorythm (returns in base64)
		/// </summary>
		/// <param name="str">source string</param>
		/// <param name="rgbKey">key vector for triple-DES algorythm (16 bytes or 128 bits)</param>
		/// <param name="rgbIV">IV vector for triple-DES algorythm (8 bytes or 64 bits)</param>
		/// <returns>encoded source string in base64</returns>
		public static string Encode (string str, string rgbKey, string rgbIV)
		{
			TripleDESCryptoServiceProvider crypto_provider = new TripleDESCryptoServiceProvider ();
			ICryptoTransform encoder = crypto_provider.CreateEncryptor (
				Encoding.Default.GetBytes (rgbKey),
				Encoding.Default.GetBytes (rgbIV));

			byte[] str_bytes = Encoding.Default.GetBytes (str);
			byte[] str_bytes_encoded = encoder.TransformFinalBlock (str_bytes, 0, str_bytes.Length);

			return Convert.ToBase64String (str_bytes_encoded);
		}

		/// <summary>
		/// Decodes string using triple-DES algorythm
		/// </summary>
		/// <param name="str">source encoded string in base64</param>
		/// <param name="rgbKey">key vector for triple-DES algorythm (16 bytes or 128 bits)</param>
		/// <param name="rgbIV">IV vector for triple-DES algorythm (8 bytes or 64 bits)</param>
		/// <returns>decoded source string</returns>
		public static string Decode (string str, string rgbKey, string rgbIV)
		{
			TripleDESCryptoServiceProvider crypto_provider = new TripleDESCryptoServiceProvider ();
			ICryptoTransform decoder = crypto_provider.CreateDecryptor (
				Encoding.Default.GetBytes (rgbKey),
				Encoding.Default.GetBytes (rgbIV));

			byte[] str_bytes = Convert.FromBase64String (str);
			byte[] str_bytes_decoded = decoder.TransformFinalBlock (str_bytes, 0, str_bytes.Length);

			return Encoding.Default.GetString (str_bytes_decoded);
		}
	}
}

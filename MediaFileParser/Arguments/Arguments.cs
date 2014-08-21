using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MediaFileParser.Arguments
{
    public class Arguments
    {
        private readonly Dictionary<string, string> _commandLineParameters;
        public Arguments(IEnumerable<string> args)
        {
            _commandLineParameters = new Dictionary<string, string>();
            var spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;

            foreach (var parts in args.Select(txt => spliter.Split(txt, 3)))
            {
                switch (parts.Length)
                {
                    case 1:
                        {
                            if (parameter != null)
                            {
                                if (!_commandLineParameters.ContainsKey(parameter))
                                {
                                    parts[0] = remover.Replace(parts[0], "$1");
                                    _commandLineParameters.Add(parameter, parts[0]);
                                }
                                parameter = null;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (parameter != null)
                            {
                                if (!_commandLineParameters.ContainsKey(parameter))
                                {
                                    _commandLineParameters.Add(parameter, "true");
                                }
                            }
                            parameter = parts[1];
                            break;
                        }
                    case 3:
                        {
                            if (parameter != null)
                            {
                                if (!_commandLineParameters.ContainsKey(parameter))
                                {
                                    _commandLineParameters.Add(parameter, "true");
                                }
                            }

                            parameter = parts[1];

                            if (!_commandLineParameters.ContainsKey(parameter))
                            {
                                parts[2] = remover.Replace(parts[2], "$1");
                                _commandLineParameters.Add(parameter, parts[2]);
                            }

                            parameter = null;
                            break;
                        }
                }
            }

            if (parameter != null && !_commandLineParameters.ContainsKey(parameter))
            {
                _commandLineParameters.Add(parameter, "true");
            }
        }

        public string this[string param]
        {
            get
            {
                try
                {
                    return _commandLineParameters[param]; 
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            set { _commandLineParameters[param] = value; }
        }

        public static bool GetBool(string param)
        {
            bool b;
            return bool.TryParse(param, out b) && b;
        }

        public bool ContainsKey(string param)
        {
            return _commandLineParameters.ContainsKey(param);
        }
    }
}

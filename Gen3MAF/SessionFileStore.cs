using Gen3MAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gen3MAF
{


    internal static class SessionFileStore
    {
        private static readonly JsonSerializerOptions s_Options =
            new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };

        static SessionFileStore()
        {
            s_Options.Converters.Add(new JsonStringEnumConverter());
        }

        public static void Save(string filePath, SessionClass session)
        {
            string json = JsonSerializer.Serialize(session, s_Options);
            File.WriteAllText(filePath, json);
        }

        public static SessionClass Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var session = JsonSerializer.Deserialize<SessionClass>(json, s_Options);

            if (session == null)
                throw new InvalidDataException("Invalid session file.");

            NormalizeAfterLoad(session);
            return session;
        }

        private static void NormalizeAfterLoad(SessionClass session)
        {
            // Defensive defaults
            session.m_TuneCycles ??= new List<TuneCycle>();

            foreach (var cycle in session.m_TuneCycles)
            {
                cycle.m_InitialAirflow ??= Array.Empty<double>();
                cycle.m_AdjustmentAirflowData ??= Array.Empty<double>();
                cycle.m_AdjustedAirflow ??= Array.Empty<double>();
            }
        }
    }
}
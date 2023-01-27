﻿using System;
using Exiled.API.Features;
using ScriptedEvents.API.Features.Actions;

namespace ScriptedEvents.Handlers.DefaultActions
{
    public class CassieAction : IAction
    {
        public string Name => "CASSIE";

        public string[] Aliases => Array.Empty<string>();

        public string[] Arguments { get; set; }

        public ActionResponse Execute()
        {
            if (Arguments.Length < 1) return new(false, "Missing text!");

            string text = string.Join(" ", Arguments);

            if (string.IsNullOrWhiteSpace(text))
                return new(false, "Missing text!");

            string[] cassieArgs = text.Split('|');
            if (cassieArgs.Length == 1)
            {
                Cassie.MessageTranslated(text, text);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(cassieArgs[0]))
                    return new(false, "Cannot show captions without a corresponding message.");

                if (string.IsNullOrWhiteSpace(cassieArgs[1]))
                    Cassie.Message(cassieArgs[0]);
                else
                    Cassie.MessageTranslated(cassieArgs[0], cassieArgs[1]);
            }

            return new(true, string.Empty);
        }
    }
}
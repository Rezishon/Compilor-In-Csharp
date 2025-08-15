using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Laxer
{
    public class BashGrammar : Grammar
    {
        public BashGrammar()
            : base(caseSensitive: true)
        {
            #region Terminals
            var Identifier = new IdentifierTerminal("Identifier");
            var Number = new NumberLiteral("Number");
            var SingleQuotedString = new StringLiteral(
                "SingleQuotedString",
                "'",
                StringOptions.AllowsAllEscapes
            );
            var DoubleQuotedString = new StringLiteral(
                "DoubleQuotedString",
                "\"",
                StringOptions.AllowsAllEscapes
            );
            #endregion

            #region KeyWords
            var Echo = ToTerm("echo");
            #endregion

            #region Operators
            var Equal = ToTerm("=");
            var Semicolon = ToTerm(";");
            var NewLine = new NewLineTerminal("NewLine");
            var WhiteSpace = ToTerm(" ");
            #endregion

            #region Comments
            var Comment = new CommentTerminal("Comment", "#", "\n", "\r\n");
            NonGrammarTerminals.Add(Comment); // Mark comments as non-grammar terminals so they are ignored by the parser
            #endregion

            #region Non-Terminals
            var Program = new NonTerminal("Program"); // The root of our grammar
            var StatementList = new NonTerminal("StatementList"); // A sequence of statements
            var Statement = new NonTerminal("Statement"); // A single command or assignment

            var Command = new NonTerminal("Command"); // Represents a generic command call
            var CommandArg = new NonTerminal("CommandArg"); // An argument to a command (string, number, identifier)
            var CommandArgList = new NonTerminal("CommandArgList");
            var EchoCommand = new NonTerminal("EchoCommand"); // Specifically for the 'echo' command

            var VariableAssignment = new NonTerminal("VariableAssignment"); // For syntax like `VAR=VALUE`
            var VariableName = new NonTerminal("VariableName"); // The name part of a variable
            var VariableValue = new NonTerminal("VariableValue"); // The value part of a variable
            #endregion

            #region Productions
            Program.Rule = StatementList | Empty;
            StatementList.Rule = MakeStarRule(StatementList, Statement);
            Statement.Rule = EchoCommand | VariableAssignment | Comment;
            MakeStarRule(CommandArgList, CommandArg);
            EchoCommand.Rule = Echo + CommandArgList;
            CommandArg.Rule = SingleQuotedString | DoubleQuotedString | Number | Identifier;
            VariableAssignment.Rule = VariableName + Equal + VariableValue;
            VariableName.Rule = Identifier;
            VariableValue.Rule = SingleQuotedString | DoubleQuotedString | Number | Identifier;
            Root = Program;
            #endregion

            #region Punctuation
            MarkPunctuation(Semicolon, NewLine, Equal);
            MarkTransient(Statement, CommandArg, VariableName, VariableValue);
            #endregion
        }
    }
}

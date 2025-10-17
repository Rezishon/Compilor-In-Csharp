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
            var Boolean = new RegexBasedTerminal("Boolean", @"\b(true|false)\b");
            // var SingleQuotedString = new StringLiteral(
            //     "SingleQuotedString",
            //     "'",
            //     StringOptions.AllowsAllEscapes
            // );
            var DoubleQuotedString = new StringLiteral(
                "DoubleQuotedString",
                "\"",
                StringOptions.AllowsAllEscapes
            );
            var Variable = new RegexBasedTerminal("Variable", @"\$[A-Za-z_][A-Za-z0-9_]*");
            var Equal = ToTerm("=");
            var Plus = ToTerm("+");
            var Minus = ToTerm("-");
            var Star = ToTerm("*");
            var Slash = ToTerm("/");
            var EqOp = ToTerm("-eq");
            var NeOp = ToTerm("-ne");
            var LtOp = ToTerm("-lt");
            var GtOp = ToTerm("-gt");
            var LeOp = ToTerm("-le");
            var GeOp = ToTerm("-ge");
            var AndAnd = ToTerm("&&");
            var OrOr = ToTerm("||");
            var Bang = ToTerm("!");
            var Echo = ToTerm("echo");
            var If = ToTerm("if");
            var Then = ToTerm("then=>");
            var Fi = ToTerm("fi");
            var Else = ToTerm("else");
            var Elif = ToTerm("elif");
            var For = ToTerm("for");
            var In = ToTerm("in");
            var Do = ToTerm("do");
            var Done = ToTerm("done");
            var While = ToTerm("while");
            var FunctionKeyword = ToTerm("function");
            var CmdPrefix = ToTerm("cmd->");
            var LParen = ToTerm("(");
            var RParen = ToTerm(")");
            var LBrace = ToTerm("{");
            var RBrace = ToTerm("}");
            var LSBrace = ToTerm("[");
            var RSBrace = ToTerm("]");
            var Semicolon = ToTerm(";");
            var NewLine = new NewLineTerminal("NewLine");
            var Comment = new CommentTerminal("Comment", "#", "\n", "\r\n");
            var Dollar = ToTerm("$");
            NonGrammarTerminals.Add(Comment);
            #endregion

            #region Non-Terminals and Rules
            // Non-Terminals represent a group of tokens that form a syntactic unit.
            var Program = new NonTerminal("Program");
            var Statement = new NonTerminal("Statement");
            var Expression = new NonTerminal("Expression");
            var AssignmentStatement = new NonTerminal("AssignmentStatement");
            var CommandStatement = new NonTerminal("CommandStatement");
            var IfStatement = new NonTerminal("IfStatement");
            var WhileStatement = new NonTerminal("WhileStatement");
            var ForStatement = new NonTerminal("ForStatement");
            var FunctionDeclaration = new NonTerminal("FunctionDeclaration");
            var Block = new NonTerminal("Block");
            var ArgumentList = new NonTerminal("ArgumentList");
            var Value = new NonTerminal("Value");
            var EchoStatement = new NonTerminal("EchoStatement");
            var StatementList = new NonTerminal("StatementList");
            var ThenSeparator = new NonTerminal("ThenSeparator");

            var VarName = Identifier;
            var VarCall = new NonTerminal("VariableCall");
            VarCall.Rule = Dollar + Identifier;

            // Define the rules for each non-terminal

            // A Value can be a number, boolean, string, or variable.
            // Value.Rule = Number | Boolean | SingleQuotedString | DoubleQuotedString | Variable;
            Value.Rule = Number | Boolean | DoubleQuotedString | Variable;

            // An Expression can be a value or a more complex arithmetic/logical expression.
            Expression.Rule =
                Value
                | Expression + Plus + Expression
                | Expression + Minus + Expression
                | Expression + Star + Expression
                | Expression + Slash + Expression
                | Expression + EqOp + Expression
                | Expression + NeOp + Expression
                | Expression + LtOp + Expression
                | Expression + GtOp + Expression
                | Expression + LeOp + Expression
                | Expression + GeOp + Expression
                | Expression + AndAnd + Expression
                | Expression + OrOr + Expression
                | Bang + Expression
                | LParen + Expression + RParen;

            AssignmentStatement.Rule = VarName + Equal + Expression;

            // A CommandStatement is a command name (Identifier) followed by a list of arguments.
            ArgumentList.Rule = MakeStarRule(ArgumentList, ToTerm(" "), Value | Identifier);
            CommandStatement.Rule = CmdPrefix + Identifier + ArgumentList;


            // An IfStatement has an expression, a `then` block, and an optional `else` block.
            IfStatement.Rule =
                If + LSBrace + Expression + RSBrace + Then + StatementList + Semicolon + Fi
             | If + LSBrace + Expression + RSBrace + Then + StatementList + Semicolon + Else + StatementList + Semicolon + Fi;
            //TODO: we can have else if or -> elif
            // | If + LSBrace + Expression + RSBrace + Semicolon + Then + Program + Elif + LSBrace + Expression + RSBrace + Semicolon + Then + Program + Fi;

            // A WhileStatement has an expression and a `do` block.
            WhileStatement.Rule = While + Expression + Do + Block + Done;

            // A ForStatement has a variable, `in`, a list of values, and a `do` block.
            ForStatement.Rule = For + Identifier + In + ArgumentList + Do + Block + Done;

            // A FunctionDeclaration is a `function` keyword (optional), an Identifier, `()`, and a block.
            FunctionDeclaration.Rule =
                FunctionKeyword + Identifier + LParen + RParen + Block
                | Identifier + LParen + RParen + Block;

            // A Block is a set of statements inside curly braces.
            Block.Rule = LBrace + StatementList + RBrace;

            // A Statement can be one of the various types defined above, or a command.
            Statement.Rule =
                AssignmentStatement
                | CommandStatement
                | IfStatement
                | WhileStatement
                | ForStatement
                | FunctionDeclaration
                | EchoStatement;
            StatementList.Rule = MakePlusRule(StatementList, NewLine | Semicolon, Statement);

            // The main Program is a list of statements separated by newlines or semicolons.
            Program.Rule = MakeStarRule(Program, NewLine | Semicolon, Statement);

            EchoStatement.Rule = Echo + Expression;

            // Set the root of the grammar to the Program.
            this.Root = Program;

            // Define operator precedence for arithmetic and logical expressions.
            RegisterOperators(10, Associativity.Left, Plus, Minus);
            RegisterOperators(20, Associativity.Left, Star, Slash);
            RegisterOperators(30, Associativity.Left, EqOp, NeOp, LtOp, GtOp, LeOp, GeOp);
            RegisterOperators(40, Associativity.Left, AndAnd, OrOr);
            RegisterOperators(50, Associativity.Left, Bang);

            // Mark special tokens for the parser.
            MarkPunctuation(
                LParen,
                RParen,
                LBrace,
                RBrace,
                LSBrace,
                RSBrace,
                Semicolon,
                NewLine,
                Then,
                Do,
                Done,
                Fi,
                Else
            );
            MarkTransient(Statement, Block, ArgumentList, Value);
            #endregion
        }
    }
}

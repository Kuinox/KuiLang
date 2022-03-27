
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

namespace KuiLang.Build
{
    public class Compile
    {
        public static IReadOnlyCollection<BuiltType>? RunCompile(string assemblyPath)
        {
            var assembly = BuildAssembly.LoadAssembly(assemblyPath);
            var compiled = assembly.Files.Select(s => KuiLang.RootRuntime.ParseFile(s)).ToArray();
            var errors = compiled.Select(s => s.ErrorValue);
            if (errors.Any())
            {
                foreach (var item in errors)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine(item);
                    return null;
                }
            }
            var allDeclarations = compiled.Select(s => s.ResultValue);
            var allBuiltTypes = allDeclarations.ToDictionary(
                s => s.TypeName,
                s => new BuiltType(s.TypeName)
            );

            foreach (var typeDeclaration in allDeclarations)
            {
                var methodsDeclaration = typeDeclaration.Fields
                    .Where(s => s.MethodDeclaration is not null)
                    .Select(s => s.MethodDeclaration!);
                var builtType = allBuiltTypes[typeDeclaration.TypeName];
                var methods = new Dictionary<string, BuiltMethod>();
                builtType.Methods = methods;
                foreach (var method in methodsDeclaration)
                {
                    methods[method.Signature.Name] = new BuiltMethod(
                        typeDeclaration.TypeName + "." + method.Signature.Name,
                        method.Signature.IsStatic,
                        allBuiltTypes[method.Signature.ReturnType.ToString()],
                        builtType
                    );
                }
            }
            return types;
        }

        IReadOnlyDictionary<string, BuiltType> _typeMap;

        public Compile(IReadOnlyDictionary<string, BuiltType> typeMap)
        {
            _typeMap = typeMap;
        }

        BuiltType ResolveType(FieldLocation location)
        {
            return ResolveType(location.Parts.Single());
        }
        BuiltType ResolveType(string typeName)
        {
            return _typeMap[typeName];
        }

        BuiltStatement CompileStatementList(IReadOnlyCollection<Statement> statements)
        {
            var builtStatements = new List<BuiltStatement>();
            foreach (var item in statements)
            {
                if (item.Expression is not null)
                {
                    builtStatements.Add(
                        new BuiltStatement(
                            CompileExpression(
                                item.Expression
                            )
                        )
                    );
                }
                else if (item.VariableDeclaration is not null)
                {
                    builtStatements.Add(
                        new BuiltStatement(
                            CompileVariableDeclaration(item.VariableDeclaration)
                        )
                    );
                }
                else if (item.StatementList is not null)
                {
                    builtStatements.Add(CompileStatementList(item.StatementList));
                }
            }
            return new BuiltStatement(builtStatements);
        }


        BuiltField ResolveField(ContextScope context, FieldLocation fieldLocation)
        {
            if (fieldLocation.Parts.Count == 1)
            {
                return context.Fields[fieldLocation.Parts[0]];
            }
            if(fieldLocation.Parts.Count == 2)
            {
                BuiltType type = _typeMap[fieldLocation.Parts[0]];
                return type.Fields[fieldLocation.Parts[1]];
            }
            throw new NotImplementedException();
        }

        BuiltMethod ResolveMethod(FieldLocation fieldLocation)
        {

        }

        BuiltExpression CompileExpression(Expression expression)
        {
            if (expression.FunctionCall is not null)
            {
                return new BuiltExpression(
                    new BuiltFunctionCall(ResolveMethod(expression.FunctionCall.FunctionRef),
                        expression.FunctionCall.Args.Select(s => CompileExpression(s)).ToArray()
                    )
                );
            }
            if (expression.FieldLocation is not null)
            {
                return new BuiltExpression(ResolveField(expression.FieldLocation));
            }
            throw new InvalidOperationException();
        }

        BuiltVariableDeclaration CompileVariableDeclaration(VariableDeclaration variableDeclaration)
        {
            return new BuiltVariableDeclaration(
                ResolveType(variableDeclaration.Type),
                variableDeclaration.Name,
                variableDeclaration.InitValue == null ? null : CompileExpression(variableDeclaration.InitValue)
            );
        }
    }
}

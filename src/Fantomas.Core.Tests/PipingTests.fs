module Fantomas.Core.Tests.PipingTests

open NUnit.Framework
open FsUnit
open Fantomas.Core.Tests.TestHelpers

// the current behavior results in a compile error since the |> is merged to the last line
[<Test>]
let ``should keep the pipe after infix operator`` () =
    formatSourceString
        false
        """
let f x =
    someveryveryveryverylongexpression
    <|> if someveryveryveryverylongexpression then someveryveryveryverylongexpression else someveryveryveryverylongexpression
    <|> if someveryveryveryverylongexpression then someveryveryveryverylongexpression else someveryveryveryverylongexpression
    |> f
    """
        { config with MaxLineLength = 80 }
    |> prepend newline
    |> should
        equal
        """
let f x =
    someveryveryveryverylongexpression
    <|> if someveryveryveryverylongexpression then
            someveryveryveryverylongexpression
        else
            someveryveryveryverylongexpression
    <|> if someveryveryveryverylongexpression then
            someveryveryveryverylongexpression
        else
            someveryveryveryverylongexpression
    |> f
"""

// the current behavior results in a compile error since the |> is merged to the last line
[<Test>]
let ``should keep the pipe after pattern matching`` () =
    formatSourceString
        false
        """let m =
    match x with
    | y -> ErrorMessage msg
    | _ -> LogMessage(msg, true)
    |> console.Write
    """
        config
    |> prepend newline
    |> should
        equal
        """
let m =
    match x with
    | y -> ErrorMessage msg
    | _ -> LogMessage(msg, true)
    |> console.Write
"""

[<Test>]
let ``should break new lines on piping`` () =
    formatSourceString
        false
        """
let runAll() =
    urlList
    |> Seq.map fetchAsync |> Async.Parallel
    |> Async.RunSynchronously |> ignore"""
        config
    |> prepend newline
    |> should
        equal
        """
let runAll () =
    urlList
    |> Seq.map fetchAsync
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
"""

[<Test>]
let ``pipe and multiline should put pipe on newline`` () =
    formatSourceString
        false
        """
let prefetchImages =
    [ playerOImage; playerXImage ]
    |> List.map (fun img -> link [ Rel "prefetch"; Href img ])"""
        config
    |> prepend newline
    |> should
        equal
        """
let prefetchImages =
    [ playerOImage; playerXImage ]
    |> List.map (fun img -> link [ Rel "prefetch"; Href img ])
"""

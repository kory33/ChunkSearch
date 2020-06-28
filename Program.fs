open System
open SilverNBTLibrary
open SilverNBTLibrary.World

open ChunkSearch

type CommandOptions =
    { withDimId : Boolean }
    static member FromOptions(options : string []) =
        { withDimId = Array.contains "--with-dim-id" options }

let entities folderPath options =
    use world = World.FromDirectory(folderPath)

    let result =
        ResizeArray
            (seq {
                for dim in world.Worlds do
                    let dimId = if options.withDimId then Some dim.DimensionID else None
                    for chunkCoord in dim.GetAllChunkCoord(false) do
                        let record =
                            { coord = Some { x = Some chunkCoord.X ; z = Some chunkCoord.Z }
                              dimensionId = dimId }
                        yield record
             })

    { result = result }

open Falanx.Proto.Codec.Binary
open Froto.Serialization
open Froto.Serialization.Serialize

type SerializerMsg =
    { message : IMessage }
    static member Serializer(m: SerializerMsg, zcb: ZeroCopyBuffer) =
        m.message.Serialize(zcb)
        zcb

[<EntryPoint>]
let main argv =
    let isOption (s : String) = s.StartsWith("--")

    let options = CommandOptions.FromOptions argv
    let worldFolderPath = (Array.skipWhile isOption argv).[0]

    let serialized = toArray { message = entities worldFolderPath options }
    use stdout = Console.OpenStandardOutput()
    stdout.Write(serialized, 0, serialized.Length)
    0

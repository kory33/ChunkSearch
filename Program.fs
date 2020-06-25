open System
open SilverNBTLibrary
open SilverNBTLibrary.World

open InventorySearch

let entities folderPath =
    use world = World.FromDirectory(folderPath)

    let result =
        ResizeArray
            (seq {
                for dim in world.Worlds do
                    for tileEntity in dim.GetAllTileEntities() do
                        let coordinate =
                            { x = Some tileEntity.XCoord
                              y = Some tileEntity.YCoord
                              z = Some tileEntity.ZCoord }

                        let nbt = tileEntity.NBTData
                        if nbt.ContainsKey "Items" || nbt.ContainsKey "RecordItem" then
                            let record =
                                { coord = Some coordinate
                                  dimensionId = Some dim.DimensionID }
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
    let serialized = toArray { message = entities argv.[0] }
    use stdout = Console.OpenStandardOutput()
    stdout.Write(serialized, 0, serialized.Length)
    0

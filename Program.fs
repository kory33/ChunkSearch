open Froto.Serialization
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

open Falanx.Proto.Codec.Binary.Primitives

[<EntryPoint>]
let main argv =
    let result = entities argv.[0]
    let buffer = ZeroCopyBuffer(int (serializedLength result))
    SearchResult.Serialize(result, buffer)
    printf "%s" (System.Text.Encoding.ASCII.GetString buffer.Array)
    0

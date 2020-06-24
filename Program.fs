open SilverNBTLibrary
open SilverNBTLibrary.World

type XYZTuple = XYZTuple of int * int * int

type DimId = DimId of int

type TileEntityId = TileEntityId of string

type TileEntity = TileEntity of XYZTuple * DimId * TileEntityId

let entities folderPath =
    use world = World.FromDirectory(folderPath)

    [
        for dim in world.Worlds do
            let dimId = DimId dim.DimensionID
            for tileEntity in dim.GetAllTileEntities() do
                let tileEntityId = TileEntityId tileEntity.Id
                let coordinate = XYZTuple(tileEntity.XCoord, tileEntity.YCoord, tileEntity.ZCoord)

                let nbt = tileEntity.NBTData
                if nbt.ContainsKey "Items" || nbt.ContainsKey "RecordItem" then
                    TileEntity(coordinate, dimId, tileEntityId)
    ]

[<EntryPoint>]
let main argv =
    entities argv.[0]
    |> List.map (printfn "%A")
    |> ignore

    0

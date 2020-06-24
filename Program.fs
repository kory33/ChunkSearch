// Learn more about F# at http://fsharp.org

open System
open SilverNBTLibrary
open SilverNBTLibrary.World

type XYZTuple = XYZTuple of int * int * int

type DimId = DimId of int

type TileEntityId = TileEntityId of string

type TileEntity = TileEntity of XYZTuple * DimId * TileEntityId

[<EntryPoint>]
let main _ =
    printfn "ワールドのフォルダのパスを入力して下さい"
    printfn "Windows10の場合はここにワールドのフォルダをドラッグ&ドロップすると楽です"

    let folderPath = Console.ReadLine()

    printfn "ワールドの読み込みを開始します"
    printfn ""

    let entities =
        use world = World.FromDirectory(folderPath)

        seq {
            for dim in world.Worlds do
                let dimId = DimId dim.DimensionID
                for tileEntity in dim.GetAllTileEntities() do
                    let tileEntityId = TileEntityId tileEntity.Id
                    let coordinate = XYZTuple(tileEntity.XCoord, tileEntity.YCoord, tileEntity.ZCoord)

                    let nbt = tileEntity.NBTData
                    if nbt.ContainsKey "Items" || nbt.ContainsKey "RecordItem" then
                        yield TileEntity(coordinate, dimId, tileEntityId)
        }

    entities
    |> Seq.map (printf "%A")
    |> ignore
    0

open System
open SilverNBTLibrary
open SilverNBTLibrary.World

type XYZTuple = XYZTuple of int * int * int

type DimId = DimId of int

type TileEntityId = TileEntityId of string

type TileEntity = TileEntity of XYZTuple * DimId * TileEntityId

let entities folderPath =
    use world = World.FromDirectory(folderPath)
    printfn "loaded world"

    [
        for dim in world.Worlds do
            let dimId = DimId dim.DimensionID
            printfn "searching for %d" (match dimId with DimId(i) -> i)
            for tileEntity in dim.GetAllTileEntities() do
                let tileEntityId = TileEntityId tileEntity.Id
                let coordinate = XYZTuple(tileEntity.XCoord, tileEntity.YCoord, tileEntity.ZCoord)

                let nbt = tileEntity.NBTData
                if nbt.ContainsKey "Items" || nbt.ContainsKey "RecordItem" then
                    TileEntity(coordinate, dimId, tileEntityId)
    ]

[<EntryPoint>]
let main _ =
    printfn "ワールドのフォルダのパスを入力して下さい"
    printfn "Windows10の場合はここにワールドのフォルダをドラッグ&ドロップすると楽です"

    let folderPath = Console.ReadLine()

    printfn "ワールドの読み込みを開始します"
    printfn ""

    entities folderPath
    |> Seq.toList
    |> List.map (printfn "%A")
    |> ignore

    0

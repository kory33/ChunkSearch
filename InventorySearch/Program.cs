using SilverNBTLibrary;
using SilverNBTLibrary.World;
using System;
using System.IO;
using System.Text;

namespace InventorySearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ワールドのフォルダのパスを入力して下さい");
            Console.WriteLine("Windows10の場合はここにワールドのフォルダをドラッグ&ドロップすると楽です");
            string folderPath = Console.ReadLine();

            Console.WriteLine("ワールドの読み込みを開始します");
            Console.WriteLine();
            var sb = new StringBuilder();
            sb.Append("X,Y,Z,ディメンションID,タイルエンティティID\n");
            using (World world = World.FromDirectory(folderPath))
            {
                foreach(var dim in world.Worlds)
                {
                    var tileEntities = dim.GetAllTileEntities();
                    foreach(var tileEntity in tileEntities)
                    {
                        var nbt = tileEntity.NBTData;
                        if (nbt.ContainsKey("Items") ||
                            nbt.ContainsKey("RecordItem"))
                        {
                            sb.AppendFormat("{0},{1},{2},{3},{4}\n", 
                                tileEntity.XCoord,
                                tileEntity.YCoord,
                                tileEntity.ZCoord,
                                dim.DimensionID,
                                tileEntity.Id);
                            Console.WriteLine(NBTFile.ToJson(tileEntity.NBTData));
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("全てのタイルエンティティの検査が終了しました");
            Console.WriteLine("CSV形式のテキストファイルに出力します");
            string file = Path.Combine(Directory.GetCurrentDirectory(), "result.txt");
            System.IO.File.WriteAllText(file, sb.ToString(), System.Text.Encoding.UTF8);

            Console.WriteLine(file + "に出力しました");
            Console.WriteLine("終了するにはなにかキーを押して下さい");
            Console.ReadKey();
        }
    }
}

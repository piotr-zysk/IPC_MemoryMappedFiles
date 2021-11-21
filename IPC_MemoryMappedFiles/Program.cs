using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace IPC_MemoryMappedFiles // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {         
            if (args.Length > 0 && args[0] == "1") Emit();
            else if (args.Length > 0 && args[0] == "2") Receive();
            else
                {
                    Console.WriteLine("IPC_MemoryMappedFiles 1   //create emitter");
                    Console.WriteLine("IPC_MemoryMappedFiles 1   //create receiver");
                }
        }

        private static void Emit()
        {
            // create a memory-mapped file of length 1000 bytes and give it a 'map name' of 'test'  
            MemoryMappedFile mmf = MemoryMappedFile.CreateNew("test", 1000);
            // write an integer value of 42 to this file at position 500  
            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();
            accessor.Write(500, 1);
            Console.WriteLine("Memory-mapped file created!");
            int x = 1;
            Console.WriteLine("Enter integer");

            while (x != 0)
            {
                try
                {
                    x = int.Parse(Console.ReadLine());
                }
                catch
                {
                    x = 0;
                }
                
                accessor.Write(500, x);
            }
            
            accessor.Dispose();
            mmf.Dispose();
        }

        private static void Receive()
        {
            Thread.Sleep(500);
            // open the memory-mapped with a 'map name' of 'test'            
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("test");
            // read the integer value at position 500  
            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();
            int x = 1;
            int y = 0;
            while (x != 0)
            {
                y = accessor.ReadInt32(500);
                if (x != y)
                {
                    x = y;
                    // print it to the console  
                    Console.WriteLine("The answer is {0}", x);
                }                
            }
           
            // dispose of the memory-mapped file object and its accessor  
            accessor.Dispose();
            mmf.Dispose();
        }
    }
}
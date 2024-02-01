using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public List<GameObject> enemiesInChunk;
    
    public int chunkWidth;
    public int chunkHeight;
    public void ChunkInit(){
        enemiesInChunk = new List<GameObject>();
    }
    public string Tostring(){
        string returnString = "Chunk Coordinate [" + chunkWidth + "] " + "[" + chunkHeight + "]";

        return returnString;
    }
}
public class Map{
    public Chunk[,] chunks; //2D chunks array
    private int mapChunkwidth;
    private int mapChunkheight;
    public void MapInit(int width, int height){
        mapChunkwidth = width;
        mapChunkheight = height; 
        chunks = new Chunk[width, height];
        for(int i = 0; i < width; i++){
            for(int j = 0; j <height; j++){
                chunks[i,j] = new Chunk();
            }
        }
    }
    public Chunk UpdateObjectChunk(GameObject someObject){
        Debug.Log("Getting Chunk");
        Vector2 position = someObject.transform.position;
        int chunkX = (int) (position.x / mapChunkwidth);
        int chunkY = (int) (position.y/mapChunkheight);
        chunks[chunkX,chunkY].enemiesInChunk.Add(someObject);
        chunks[chunkX,chunkY].chunkHeight = chunkY;
        chunks[chunkX,chunkY].chunkWidth = chunkX;

        return chunks[chunkX, chunkY];
    }

}

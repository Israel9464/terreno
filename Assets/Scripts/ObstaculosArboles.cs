using UnityEngine;
using UnityEngine.AI;

public class ObstaculosArboles : MonoBehaviour
{
    void Start()
    {
        Terrain terreno = GetComponent<Terrain>();
        if (terreno == null) return;

        TerrainData datos = terreno.terrainData;

        foreach (TreeInstance arbol in datos.treeInstances)
        {
            // Calcula la posición exacta de cada árbol pintado en el mapa
            Vector3 posMundo = Vector3.Scale(arbol.position, datos.size) + terreno.transform.position;

            GameObject obsObj = new GameObject("ObstaculoArbol");
            obsObj.transform.position = posMundo;
            obsObj.transform.parent = transform;

            // Crea un cilindro invisible que recorta el NavMesh alrededor del tronco
            NavMeshObstacle obstaculo = obsObj.AddComponent<NavMeshObstacle>();
            obstaculo.shape = NavMeshObstacleShape.Capsule;
            obstaculo.radius = 0.3f * arbol.widthScale;
            obstaculo.height = 4f;
            obstaculo.carving = true;
        }
    }
}

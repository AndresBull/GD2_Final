using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GridComponent
{
    private static List<GameObject> GetCollidingObjects(GameObject gridComponent)
    {
        Vector3 gridComponentDimensions =(gridComponent.transform.localScale);

        Collider[]colliders = Physics.OverlapBox(gridComponent.transform.position, gridComponentDimensions);
        
        List<GameObject> colliderGameObjects = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            colliderGameObjects.Add(collider.gameObject);
        }

        return colliderGameObjects;
    }

    private static bool ContainsGameObjectInLayer(GameObject gridComponent, LayerMask layerMask,out GameObject collidingGameObject)
    {
        List<GameObject> allCollidingGameObjects = GetCollidingObjects(gridComponent);
        foreach (GameObject gameObject in allCollidingGameObjects)
        {
            if (gameObject.layer == layerMask)
            {
                collidingGameObject = gameObject;
                return true;
            }
        }

        collidingGameObject = null;
        return false;
    }
}

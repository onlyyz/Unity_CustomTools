using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace MU5Editor.NodeEditor
{
    public class SearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private MyGraphView graphView;
        private GraphWindow graphWindow;

        public void Initialize(MyGraphView graphView, GraphWindow graphWindow)
        {
            this.graphView = graphView;
            this.graphWindow = graphWindow;
        }

        //Search Tree Entry
        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && (type.IsSubclassOf(typeof(MU5Node)))
                        && type != typeof(EntryNode) && type != typeof(ExitNode))
                    {
                        
                        
                        entries.Add(new SearchTreeEntry(new GUIContent(type.Name))
                        {
                            level = 1, userData = type
                        });
                    }
                }
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as System.Type;

            var node = Activator.CreateInstance(type) as MU5Node;
            var worldMousePosition = graphWindow.rootVisualElement.ChangeCoordinatesTo(graphWindow.rootVisualElement.parent, context.screenMousePosition - graphWindow.position.position);
            var localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);
            node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));
            graphView.AddElement(node);

            return true;
        }
    }
}

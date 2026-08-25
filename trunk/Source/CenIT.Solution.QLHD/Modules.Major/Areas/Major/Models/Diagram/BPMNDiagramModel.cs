using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Modules.Major.Areas.Major.Models.Diagram
{
    [XmlRoot(ElementName = "definitions")]
    public class DefinitionModel
    {
        [XmlElement(ElementName = "process")]
        public Process Process;

        [XmlElement(ElementName = "BPMNDiagram")]
        public BPMNDiagramModel BPMNDiagram;

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi;

        [XmlAttribute(AttributeName = "bpmn")]
        public string Bpmn;

        [XmlAttribute(AttributeName = "bpmndi")]
        public string Bpmndi;

        [XmlAttribute(AttributeName = "dc")]
        public string Dc;

        [XmlAttribute(AttributeName = "di")]
        public string Di;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "targetNamespace")]
        public string TargetNamespace;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "BPMNDiagram")]
    public class BPMNDiagramModel
    {

        [XmlElement(ElementName = "BPMNPlane")]
        public BPMNPlaneModel BPMNPlane;

        [XmlAttribute(AttributeName = "id")]
        public string Id;
    }

    [XmlRoot(ElementName = "BPMNPlane")]
    public class BPMNPlaneModel
    {

        [XmlElement(ElementName = "BPMNShape")]
        public List<BPMNShapeModel> BPMNShape;

        [XmlElement(ElementName = "BPMNEdge")]
        public List<BPMNEdgeModel> BPMNEdge;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "bpmnElement")]
        public string BpmnElement;
    }

    [XmlRoot(ElementName = "BPMNEdge")]
    public class BPMNEdgeModel
    {

        [XmlElement(ElementName = "waypoint")]
        public List<WaypointModel> Waypoint;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "bpmnElement")]
        public string BpmnElement;
    }

    [XmlRoot(ElementName = "waypoint")]
    public class WaypointModel
    {

        [XmlAttribute(AttributeName = "x")]
        public int X;

        [XmlAttribute(AttributeName = "y")]
        public int Y;
    }

    [XmlRoot(ElementName = "BPMNShape")]
    public class BPMNShapeModel
    {

        [XmlElement(ElementName = "Bounds")]
        public BoundModel Bounds;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "bpmnElement")]
        public string BpmnElement;

        [XmlElement(ElementName = "BPMNLabel")]
        public object BPMNLabel;
    }

    [XmlRoot(ElementName = "startEvent")]
    public class StartEventModel
    {

        [XmlElement(ElementName = "outgoing")]
        public string Outgoing;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "task")]
    public class TaskModel
    {

        [XmlElement(ElementName = "incoming")]
        public string Incoming;

        [XmlElement(ElementName = "outgoing")]
        public string Outgoing;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "name")]
        public string Name;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "sequenceFlow")]
    public class SequenceFlowModel
    {

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "sourceRef")]
        public string SourceRef;

        [XmlAttribute(AttributeName = "targetRef")]
        public string TargetRef;
    }

    [XmlRoot(ElementName = "endEvent")]
    public class EndEventModel
    {

        [XmlElement(ElementName = "incoming")]
        public string Incoming;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "process")]
    public class ProcessModel
    {

        [XmlElement(ElementName = "startEvent")]
        public StartEventModel StartEvent;

        [XmlElement(ElementName = "task")]
        public List<TaskModel> Task;

        [XmlElement(ElementName = "sequenceFlow")]
        public List<SequenceFlowModel> SequenceFlow;

        [XmlElement(ElementName = "endEvent")]
        public EndEventModel EndEvent;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "isExecutable")]
        public bool IsExecutable;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "Bounds")]
    public class BoundModel
    {

        [XmlAttribute(AttributeName = "x")]
        public int X;

        [XmlAttribute(AttributeName = "y")]
        public int Y;

        [XmlAttribute(AttributeName = "width")]
        public int Width;

        [XmlAttribute(AttributeName = "height")]
        public int Height;
    }
}
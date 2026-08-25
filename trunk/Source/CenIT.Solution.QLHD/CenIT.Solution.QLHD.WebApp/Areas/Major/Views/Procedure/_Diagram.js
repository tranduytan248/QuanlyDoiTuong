//debugger;

var bpmnModeler = null;

function _initBpmnJS(isEdit, container, eventChose) {   
   
    if(isEdit){
        bpmnModeler = new BpmnJS({
            container: container,
            keyboard: {
                bindTo: window
            }
        });
        bpmnModeler.createDiagram();
    }else{
        bpmnModeler = new BpmnJS({
            container: container,
        });
    }

    bpmnModeler.on('shape.added', (e) => {
        if (e.element.type === 'bpmn:Task') {
            bpmnModeler.get('canvas').addMarker(e.element.id, 'stroke-task');
        }else if (e.element.type === 'bpmn:StartEvent') {
            bpmnModeler.get('canvas').addMarker(e.element.id, 'startEvent');
        }else if (e.element.type === 'bpmn:EndEvent') {
            bpmnModeler.get('canvas').addMarker(e.element.id, 'endEvent');
        }
    });

    eventChose == null ? _onEventHandler() : eventChose();
}

function _createDiagram() {
    bpmnModeler.createDiagram();
}

async function _exportDiagram() {
    var result = await bpmnModeler.saveXML({ format: true });
    return result.xml;
}

function _openDiagram(bpmnXML) {
    bpmnModeler.importXML(bpmnXML,function () {
        //access modeler components
        var canvas = bpmnModeler.get('canvas');

        // zoom to fit full viewport
        canvas.zoom("fit-viewport", "auto");
    });
    // access modeler components
    //var canvas = bpmnModeler.get('canvas');

    //// zoom to fit full viewport
    //canvas.zoom("fit-viewport", "auto");
}

function _makerNotConfig(elementId) {
    var overlays = bpmnModeler.get('overlays');

    var $overlayHtml =
            $('<div class="highlight-red">Chưa cấu hình!</div>')
            .css({
                width: 100,
                height: 25
            });

    overlays.add(elementId, {
        position: {
            top: 55,
            left: 0
        },
        html: $overlayHtml
    });
}

function _onEventHandler(typeElement) {
    typeElement = typeElement == null || typeElement.length == 0 ? 'bpmn:Task' : typeElement;
    const events = [
      // "commandStack.elements.create.postExecuted",
      // "commandStack.elements.delete.postExecuted",
      // "commandStack.elements.move.postExecuted",
      // "elements.changed",
      // "elements.delete",
      // "element.changed"
      "element.click",
      // "element.dblclick"
    ];

    var overlays = bpmnModeler.get('overlays');
    var elementRegistry = bpmnModeler.get('elementRegistry');
    var eventBus = bpmnModeler.get("eventBus");
    var canvas = bpmnModeler.get('canvas');

    eventBus.on([...events], function (e) {
        // e.element = the model element
        // e.gfx = the graphical element
        const taskElement = elementRegistry.get(e.element.id);
        var elements = elementRegistry.filter(function (element) {
            return element.type == typeElement;
        });
        $.each(elements, function (idx, element) {
            switch (event.type) {
                case "click":
                    if (element.id != e.element.id) {
                        canvas.removeMarker(element.id, 'highlight');
                    } else {
                        canvas.addMarker(e.element.id, 'highlight');
                    }
                    break;
            }

        });
    });
};

function _resetView() {
    bpmnModeler.get('zoomScroll').reset();
}

function _zoomIn() {
    bpmnModeler.get('zoomScroll').stepZoom(1);
}

function _zoomOut() {
    bpmnModeler.get('zoomScroll').stepZoom(-1);
}
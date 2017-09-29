package hr.fer.zemris.ooup.lab4.state;

import hr.fer.zemris.ooup.lab4.model.GraphicalObject;
import hr.fer.zemris.ooup.lab4.model.Point;
import hr.fer.zemris.ooup.lab4.redner.Renderer;

public interface State {
    void mouseDown(Point mousePoint, boolean shiftDown, boolean ctrlDown);

    void mouseUp(Point mousePoint, boolean shiftDown, boolean ctrlDown);

    void mouseDragged(Point mousePoint);

    void keyPressed(int keyCode);

    void afterDraw(Renderer r, GraphicalObject go);

    void afterDraw(Renderer r);

    void onLeaving();
}

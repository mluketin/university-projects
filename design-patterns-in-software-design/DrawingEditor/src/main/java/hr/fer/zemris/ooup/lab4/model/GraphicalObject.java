package hr.fer.zemris.ooup.lab4.model;


import java.util.List;
import java.util.Stack;

import hr.fer.zemris.ooup.lab4.redner.Renderer;

public interface GraphicalObject {

    boolean isSelected();

    void setSelected(boolean selected);

    int getNumberOfHotPoints();

    Point getHotPoint(int index);

    void setHotPoint(int index, Point point);

    boolean isHotPointSelected(int index);

    void setHotPointSelected(int index, boolean selected);

    double getHotPointDistance(int index, Point mousePoint);

    void translate(Point delta);

    Rectangle getBoundingBox();

    double selectionDistance(Point mousePoint);

    void addGraphicalObjectListener(GraphicalObjectListener l);

    void removeGraphicalObjectListener(GraphicalObjectListener l);

    String getShapeName();

    GraphicalObject duplicate();

    void render(Renderer r);

    String getShapeID();

    void save(List<String> rows);

    void load(Stack<GraphicalObject> stack, String data);
}

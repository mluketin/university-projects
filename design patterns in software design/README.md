Design Patterns in Software Design (https://www.fer.unizg.hr/en/course/dpisd)

DrawingEditor project is Java swing application.

Application has to enable user to:
	- add geometrical shapes like lines and elipses
	- group shapes
	- delete
	- change sequence of drawing (important for overlaping shapes)
	- modifying objects (translation, resize)
	- saving and loading drwaing in "nativ" format
	- export into SVG file
	
Folowing design patterns are used for this application:
	- Observer - describes relationship between data model and drawn components
	- Composite - enables transparent execution of actions on single and grouped elements
	- Iterator - iterating through elements of drawing
	- Prototype - enables creation of toolbar for adding new shapes and is not dependent on current concrete geometrical shapes
	- Factory - creation of concrete shapes based on shape's symbolic name
	- State - enables adding new tools without modifying component which is processing user input (mouse, keyboard)
	- Bridge - used for transparent rendering of shapes and exporting into different image formats like SVG
/**
 * Comp 585
 * Sierpinski Attractor  Read Me
 * 
 * Kyle Dumo kyle.dumo.789@my.csun.edu
 * Joseph Pena joseph.pena.943@my.csun.edu
 * 
 */
 
 Color Setting: use 3 combo boxes to select color.  One for each color, R,G,B.
				Since each color can have different values, a combo box was a better choice
				over the radio button because there would be too many radio buttons.
 
 Size Setting:  used 3 radio buttons to selece size.  One for small, medium, large setting.
				This would be the better choice to select size since there were not many options.

 Sierpinski Attractor: used a canvas to hold the points.
 
 Layout uses the grid layout because it was easy to align and move elements around.
 
 Event Diagram
->Select Size and Color ->Add Point	-> if less than 3 points -> repeat - color and size selection
					-> if more than 6 points -> no more points can be added
					-> if between 3 and 6 -> run -> run antilogarithm -> draw
					-> if point selected -> edit point color, size and drag to new position -> if running -> run algorithm and redraw
 
 
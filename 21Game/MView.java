import java.awt.*;
import java.awt.event.*;

// machine view for displaying the state of a machine player 

class MView extends MLayout implements IDisplay {
    MView(String name) { super(name); }

    // ------------------------------------------------------------------

    public void sumDisplay(int s) {
	sump.setText(sums(s));
    }
    
    public void skipsDisplay (int sk) {
    skipsp.setText(skips(sk));
    }

    public void msgDisplay(String s) {
	msgp.setText(s);
    }

    // ------------------------------------------------------------------
    // Examples: 
    static MView v1;

    static public void createExamples() {
	if (v1 == null) {
	    v1 = new MView("matthew");
	}
    }

    // ------------------------------------------------------------------
    // Test
    public static void main(String argv[]) {
	try {
	    createExamples();
	    Robot r = new Robot(); 
	    v1.sumDisplay(77); 
	    v1.msgDisplay("77");
	    
	    // just to show what one can test
	    System.out.println("Check for seventy-seven (77)");
	    r.setAutoDelay(3000);
	    
	    v1.quit(r); 
	}
	catch (AWTException e) {
	    System.out.println("awt exception thrown"); 
	};
	    
    }

}

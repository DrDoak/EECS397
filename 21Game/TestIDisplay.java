// a simple implementation of the IDisplay interface 

class TestIDisplay implements IDisplay {

    public void sumDisplay(int s) {
	System.out.println("sum = " + s); 
    }
    
    public void skipsDisplay(int sk) {
    System.out.println("skips = " + sk);
    }

    public void msgDisplay(String s) {
	System.out.println("msg = " + s); 
    }

    // a useful instance 
    static public IDisplay testIDisplay = new TestIDisplay();
}


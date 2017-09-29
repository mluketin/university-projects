package hr.fer.avsp.laboratory.lab3;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.List;


public class DGIM {

  public static class Bucket {
    	public int size;
    	public int timestamp;

    	public Bucket(int size, int timestamp){
    		this.size = size;
    		this.timestamp = timestamp;
    	}	
    	
    	@Override
    	public String toString() {
    	return "("+size + ", " + timestamp + ")";
    	}
    }

    private static int N;
    private static int timestamp;
    private static List<Bucket> compartments = new ArrayList<>();

    
    public static void main(String[] args) throws NoSuchAlgorithmException, IOException {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

		//size of window
        N = Integer.parseInt(br.readLine());
                
        String line = null;
        while((line = br.readLine()) != null) {
        	if(line.charAt(0) == 'q'){
				//query
        		int k = Integer.valueOf(line.split(" ")[1]);
        		int answer = query(k);
        		System.out.println(answer);
        		String a = String.valueOf(answer) + "\n";
        	} else {
				//stream
        		char[] stream = line.toCharArray();
        		for(int i = 0; i < stream.length; i++){
        			if(stream[i] == '1'){
        				Bucket bucket = new Bucket(1, timestamp);
        				compartments.add(bucket);
        				recombineCompartment();
        			}
        			refreshCompartment();
        			timestamp++;
        		}
        	}
        }
        br.close();
    }


	private static int query(int k) {
		int length = compartments.size();
		int count = 0;
		boolean found = false;
		int targetIndex = 0;
		for(int i = length-1; i >= 0; i--){
			if(compartments.get(i).timestamp >= (timestamp - k)){
				found = true;
				targetIndex = i;
			} else {
				break;
			}
		}
		if(found == false) return 0;
		for(int i = length-1; i >= 0; i--){
			if(i != targetIndex){
				count += compartments.get(i).size;
			} else {
				count += compartments.get(i).size / 2;
				break;
			}
		}
		return count;
	}


	private static void refreshCompartment() {
		if(compartments.size() > 0){
			if(compartments.get(0).timestamp <= (timestamp - N)){
				compartments.remove(0);
			}
		}
	}


	private static void recombineCompartment() {
		int length = compartments.size();
		int currentSize = 1;
		int currentCounter = 0;
		for(int i = length-1; i >= 0; i--){
			if(compartments.get(i).size == currentSize){
				currentCounter++;
			}
			if(currentCounter == 3){
				int newTimestamp = compartments.get(i+1).timestamp;
				
				Bucket newBucket = new Bucket(currentSize*2, newTimestamp);
				compartments.remove(i+1);
				compartments.remove(i);
				compartments.add(i, newBucket);
				
				currentSize *= 2;
				currentCounter = 1;
			}
		}
	}

}

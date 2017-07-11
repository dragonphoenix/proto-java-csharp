package com.dragon.proto.utils;

public class Random {

	public static String randStr(String seed, int len){
		int length = seed.length();
		java.util.Random random = new java.util.Random();
		
		if(len <= 0)
			return "";
		
		String ret = "";
		int index;
		for(int i=0; i<len; i++){
			index = random.nextInt(length);
			ret += seed.charAt(index);
		}
		
		return ret;
	}
	
	public static String randPhoneNum(){
		String num = "5678912340";
		return randStr(num, 8);
	}
}

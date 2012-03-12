function depthPercent  = f_squatDepth(obj, frameNumber)
	fl=obj.getJointData(frameNumber, 'FOOT_L');
	fr=obj.getJointData(frameNumber, 'FOOT_R');
	hipc=obj.getJointData(frameNumber, 'HIP_C');
	
	hipHeight=((hipc(2)-fl(2)) + (hipc(2)-fr(2)))/2;
	depthPercent=100*(hipHeight/obj.calibData.userDetails.hipHeight);

end



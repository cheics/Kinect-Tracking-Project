function depthPercent  = f_headDepth(obj, frameNumber)
	head=obj.getJointData(frameNumber, 'HEAD');
	fl=obj.getJointData(frameNumber, 'FOOT_L');
	fr=obj.getJointData(frameNumber, 'FOOT_R');
	
	headHeight=((head(2)-fl(2)) + (head(2)-fr(2)))/2;
	depthPercent=100*(headHeight/obj.calibData.userDetails.hipHeight);

end



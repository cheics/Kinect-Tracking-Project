function [hipHeight, headHeight] = calibHeights(obj)
	frameNumber=1;
	head=obj.getJointData(frameNumber, 'HEAD');
	fl=obj.getJointData(frameNumber, 'FOOT_L');
	fr=obj.getJointData(frameNumber, 'FOOT_R');
	hipc=obj.getJointData(frameNumber, 'HIP_C');
	
	hipHeight=((hipc(2)-fl(2)) + (hipc(2)-fr(2)))/2;
	headHeight=((head(2)-fl(2)) + (head(2)-fr(2)))/2;
end
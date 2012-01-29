function jointXYZ = getJointData(obj, frameNumber, jointName)
	jointXYZ=obj.skelData(frameNumber, :, obj.jnts.(jointName));
end
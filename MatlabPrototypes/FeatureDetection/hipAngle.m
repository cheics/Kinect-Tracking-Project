function [ angle ] = hipAngle( hipCentre, hipRight, knee )
    
    ba=hipRight-hipCentre;
    bc=hipRight-knee;
    
    angle=acos(dot(ba./norm(ba),bc./norm(bc)))*180/pi;

end



function [score,label] = GEDClassifier(x,ug,ubb,ubh,covarg,covarbb,covarbh)

      dGC = (x - ug) * (inv(covarg)) * transpose(x - ug);
      dGD = (x - ubb) * (inv(covarbb)) * transpose(x - ubb);
      dGE = (x - ubh) * (inv(covarbh)) * transpose(x - ubh);

      dC = dGC(1)^0.5;
      dD = dGD(1)^0.5;
      dE = dGE(1)^0.5;

      if (dC < dD) && (dC < dE)
        label = 3;
        score = dC;
      elseif (dD < dC) && (dD < dE)
        label = 2;
        score = dD;
      else
        label = 1;
        score = dE;
      end
end


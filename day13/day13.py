# -*- coding: utf-8 -*-
"""
Created on Tue Dec 13 19:29:15 2022

@author: huish
"""

import functools
import json

from typing import List


def wrap(value):
    if type(value) == int:
        return [value]
    else:
        return value
    

def compare(list1, list2):
    for ii in range(len(list1)):
        if ii >= len(list2):
            return False
        
        v1 = list1[ii]
        v2 = list2[ii]

        if type(v1) == int and type(v2) == int:
            if v1 < v2:
                return True
            if v1 > v2:
                return False
            continue
    
        result = compare(wrap(v1), wrap(v2))
        if result is not None:
            return result
        
    if len(list1) < len(list2):
        return True
    else:
        return None


def compareInt(list1, list2):
    boolval = compare(list1, list2)
    if boolval is None:
        return 0
    elif boolval is True:
        return 1
    elif boolval is False:
        return -1


def ordered(str1: str, str2: str) -> bool:
    res1 = json.loads(str1.strip())
    res2 = json.loads(str2.strip())
    
    in_right_order = compare(res1, res2)
    
    return in_right_order


def get_index(packet: str, packet_list: List):
    index = 1
    for other_packet in packet_list:
        is_larger = ordered(other_packet, packet)
        if is_larger:
            index += 1
    return index


if __name__ == "__main__":
    example_input = [
        "[1,1,3,1,1]",
        "[1,1,5,1,1]",    
        "",
        "[[1],[2,3,4]]",
        "[[1],4]",
        "",
        "[9]",
        "[[8,7,6]]",
        "",
        "[[4,4],4,4]",
        "[[4,4],4,4,4]",
        "",
        "[7,7,7,7]",
        "[7,7,7]",
        "",
        "[]",
        "[3]",
        "",
        "[[[]]]",
        "[[]]",
        "",
        "[1,[2,[3,[4,[5,6,7]]]],8,9]",
        "[1,[2,[3,[4,[5,6,0]]]],8,9]",
        ""
    ]
    
    with open(r"C:\Users\huish\Documents\CodeSnips\adventofcode2022\day13\input.txt") as infile:
        real_input = infile.read().split("\n")

    input = real_input  # example_input
    
    n_pairs = int(len(input) / 3)
    
    # Part 1: Determine the number of pairs that are ordered
    ordered_idx = []
    
    for ii in range(n_pairs):        
        str1 = input[3 * ii]
        str2 = input[3 * ii + 1]
        
        res = ordered(str1, str2)
        if res:
            ordered_idx.append(ii + 1)
    
    # print(ordered_idx)
    res1 = sum(ordered_idx)
    print("Res 1: %i" % res1)
    
    # Part 2: 
    # Return the product of the indices of the divider packets 
    
    divider_packets = [
        "[[2]]",
        "[[6]]"
    ]

    input_drop_empty = [line for line in input if line != ""]   
    res2 = 1
    for (jj, div_pack) in enumerate(divider_packets):
        full_list = input_drop_empty + divider_packets[:jj]
        idx = get_index(div_pack, full_list)
        res2 *= idx
        print(idx)
    
    print("Res 2: %i" % res2)
